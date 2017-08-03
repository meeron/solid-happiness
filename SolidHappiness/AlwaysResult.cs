using System;
using System.Linq;
using System.Linq.Expressions;
using SolidHappiness.Impl;

namespace SolidHappiness
{
    public static class AlwaysResult<T>
        where T: class
    {
        private static IStorage _storage = new MemoryStorage();

        public static TResult Call<TResult>(Expression<Func<T, TResult>> expression, T obj)
        {
            var methodExpression = (expression.Body as MethodCallExpression);
            var parameters = methodExpression.Arguments.Select(a =>
                LabelExpression.Lambda(a).Compile().DynamicInvoke()).ToArray();

            var key = $"{typeof(T)}.{methodExpression.Method.Name}_{string.Join(",", parameters)}";

            try
            {
                _storage.Set(key, methodExpression.Method.Invoke(obj, parameters));
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
