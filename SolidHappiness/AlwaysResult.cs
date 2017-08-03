using System;

namespace SolidHappiness
{
    public static class AlwaysResult<T>
        where T: class
    {
        public static TResult Call<TResult>(Func<T, TResult> methodExpression)
        {
            throw new NotImplementedException();
        }
    }
}
