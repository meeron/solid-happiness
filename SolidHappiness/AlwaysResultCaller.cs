using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SolidHappiness
{
    public class AlwaysResultCaller<T>
        where T: class
    {
        private readonly IStorage _storage;

        private readonly T _obj;

        private Action<Exception> _exceptionHandler;

        internal AlwaysResultCaller(IStorage storage, T obj)
        {
            _storage = storage;
            _obj = obj;
        }

        public AlwaysResultCaller<T> WithExceptionHandler(Action<Exception> handler)
        {
            _exceptionHandler = handler;
            return this;
        }

        public TResult Invoke<TResult>(Expression<Func<T, TResult>> expression)
        {
            var methodExpression = (expression.Body as MethodCallExpression);
            var parameters = methodExpression.Arguments.Select(a =>
                LabelExpression.Lambda(a).Compile().DynamicInvoke()).ToArray();

            var key = $"{typeof(T)}.{methodExpression.Method.Name}_{string.Join(",", parameters)}";

            try
            {
                _storage.Set(key, methodExpression.Method.Invoke(_obj, parameters));
            }
            catch (System.Exception ex)
            {
                if (!_storage.Exists(key))
                    throw ex;

                if (_exceptionHandler != null)
                    _exceptionHandler(ex);                    
            }

            return (TResult)_storage.Get(key);
        }
    }
}