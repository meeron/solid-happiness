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

        internal AlwaysResultCaller(IStorage storage, T obj)
        {
            _storage = storage;
            _obj = obj;
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
            }

            return (TResult)_storage.Get(key);
        }
    }
}