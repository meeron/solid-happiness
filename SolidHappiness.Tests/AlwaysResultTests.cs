using System;
using Xunit;
using SolidHappiness;

namespace SolidHappiness.Tests
{
    public class AlwaysResultTests
    {
        [Fact]
        public void Call_Method_Without_Parameters_And_Get_Result()
        {
            var result1 = AlwaysResult<MockClass>.Call(c => c.Method1(), new MockClass(false));
            var result2 = AlwaysResult<MockClass>.Call(c => c.Method1(), new MockClass(true));

            Assert.Equal(result1, result2);
        }

        [Fact]
        public void Call_Method_With_Parameter_And_Get_Result()
        {
            var result11 = AlwaysResult<MockClass>.Call(c => c.Method2("test1", 1), new MockClass(false));
            var result12 = AlwaysResult<MockClass>.Call(c => c.Method2("test2", 2), new MockClass(false));

            var result21 = AlwaysResult<MockClass>.Call(c => c.Method2("test1", 1), new MockClass(true));
            var result22 = AlwaysResult<MockClass>.Call(c => c.Method2("test2", 2), new MockClass(true));

            Assert.Equal(result11, result21);
            Assert.Equal(result12, result22);           
        }
    }

    public class MockClass
    {
        private readonly bool _throwOnCall;

        public MockClass(bool throwOnCall)
        {
            _throwOnCall = throwOnCall;
        }

        public string Method1()
        {
            if (_throwOnCall)
                throw new Exception("Test");

            return Guid.NewGuid().ToString();
        }

        public string Method2(string param1, int param2)
        {
            if (_throwOnCall)
                throw new Exception("Test");

            return $"{Guid.NewGuid()}_{param1}_{param2}";
        }        
    }
}
