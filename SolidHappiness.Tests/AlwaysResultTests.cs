using System;
using Xunit;
using SolidHappiness;

namespace SolidHappiness.Tests
{
    public class AlwaysResultTests
    {
        [Fact]
        public void Test_Method_Without_Parameters()
        {
            bool mockResult = AlwaysResult<MockClass>.Call(c => c.Method1());
        }
    }

    public class MockClass
    {
        public bool Method1()
        {
            return false;
        }
    }
}
