using System;
using System.Linq;
using System.Linq.Expressions;
using SolidHappiness.Impl;

namespace SolidHappiness
{
    public static class AlwaysResult
    {
        private static IStorage _storage = new MemoryStorage();

        public static AlwaysResultCaller<T> For<T>(T obj)
            where T: class
        {
            return new AlwaysResultCaller<T>(_storage, obj);
        }
    }
}
