using System;
using System.Linq.Expressions;

namespace SolidHappiness
{
    public static class AlwaysResult<T>
        where T: class
    {
        private static IStorage _storage = new MemoryStorage();

        public static TResult Call<TResult>(Expression<Func<T, TResult>> expression, T obj)
        {
            var methodExpression = (expression.Body as MethodCallExpression);
            var key = $"{typeof(T)}.{methodExpression.Method.Name}";

            try
            {
                _storage.Set(key, methodExpression.Method.Invoke(obj, new object[] {}));
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
