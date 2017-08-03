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
            bool mockResult = AlwaysResult<MockClass>.Call(c => c.Method1(), new MockClass());

            Assert.True(mockResult);
        }
    }

    public class MockClass
    {
        public bool Method1()
        {
            return true;
        }
    }
}
