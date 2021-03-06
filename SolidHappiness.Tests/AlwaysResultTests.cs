using System;
using System.Linq;
using Xunit;
using SolidHappiness;
using NSubstitute;
using System.Threading.Tasks;

namespace SolidHappiness.Tests
{
    public class AlwaysResultTests
    {
        [Fact]
        public void Call_Method_Without_Parameters_And_Get_Result()
        {
            var mock = Substitute.For<IMockClass>();

            mock.Method1().Returns(Guid.NewGuid().ToString());
            var result1 = AlwaysResult.For<IMockClass>(mock).Invoke(m => m.Method1());

            mock.Method1().Returns(x => { throw new Exception(); });
            var result2 = AlwaysResult.For<IMockClass>(mock).Invoke(m => m.Method1());

            Assert.Equal(result1, result2);
        }

        [Fact]
        public void Call_Method_With_Parameter_And_Get_Result()
        {
            var paramStr1 = "test1";
            var paramStr2 = "test2";
            var paramInt1 = 1;
            var paramInt2 = 2;

            var mock = Substitute.For<IMockClass>();

            mock.Method2(paramStr1, paramInt1).Returns($"{Guid.NewGuid()}_{paramStr1}_{paramInt1}");
            var result11 = AlwaysResult.For<IMockClass>(mock).Invoke(c => c.Method2(paramStr1, paramInt1));

            mock.Method2(paramStr2, paramInt2).Returns($"{Guid.NewGuid()}_{paramStr2}_{paramInt2}");
            var result12 = AlwaysResult.For<IMockClass>(mock).Invoke(c => c.Method2(paramStr2, paramInt2));

            mock.Method2(paramStr1, paramInt1).Returns(x => { throw new Exception(); });
            var result21 = AlwaysResult.For<IMockClass>(mock).Invoke(c => c.Method2(paramStr1, paramInt1));

            mock.Method2(paramStr2, paramInt2).Returns(x => { throw new Exception(); });
            var result22 = AlwaysResult.For<IMockClass>(mock).Invoke(c => c.Method2(paramStr2, paramInt2));

            Assert.Equal(result11, result21);
            Assert.Equal(result12, result22);           
        }

        [Fact]
        public void Call_Method_With_ExceptionHandler()
        {
            var mock = Substitute.For<IMockClass>();

            mock.Method2("test", 1).Returns("test");
            AlwaysResult.For<IMockClass>(mock).Invoke(c => c.Method2("test", 1));

            mock.Method2("test", 1).Returns(x => { throw new Exception(); });
            AlwaysResult.For<IMockClass>(mock)
                .WithExceptionHandler((ex) => mock.Method1())
                .Invoke(c => c.Method2("test", 1));

            mock.Received().Method1();
        }

        [Fact]
        public async Task Call_Async_Methods_Without_Parameters()
        {
            var mock1 = Substitute.For<IMockClass>();
            mock1.Method3Async().Returns(Guid.NewGuid().ToString());
            var mock2 = Substitute.For<IMockClass>();
            mock2.Method3Async().Returns(Task<string>.Factory.StartNew(() => throw new Exception()));     
            
            var result1Task = AlwaysResult.For<IMockClass>(mock1).InvokeAsync(c => c.Method3Async());            
            var result2Task = AlwaysResult.For<IMockClass>(mock2).InvokeAsync(c => c.Method3Async());

            Assert.Equal(await result1Task, await result2Task);
        }

        [Fact]
        public async Task Call_Async_Methods_With_Parameters()
        {
            var mock1 = Substitute.For<IMockClass>();
            mock1.Method4Async(1, 2).Returns(Guid.NewGuid().ToString());
            var mock2 = Substitute.For<IMockClass>();
            mock2.Method4Async(3, 4).Returns(Guid.NewGuid().ToString());
            var mock3 = Substitute.For<IMockClass>();
            mock3.Method4Async(1, 2).Returns(Task<string>.Factory.StartNew(() => throw new Exception()));
            var mock4 = Substitute.For<IMockClass>();
            mock4.Method4Async(3, 4).Returns(Task<string>.Factory.StartNew(() => throw new Exception()));     
            
            var result1 = await AlwaysResult.For<IMockClass>(mock1).InvokeAsync(c => c.Method4Async(1, 2));
            var result2 = await AlwaysResult.For<IMockClass>(mock2).InvokeAsync(c => c.Method4Async(3, 4));
            var result3 = await AlwaysResult.For<IMockClass>(mock3).InvokeAsync(c => c.Method4Async(1, 2));
            var result4 = await AlwaysResult.For<IMockClass>(mock4).InvokeAsync(c => c.Method4Async(3, 4));

            Assert.Equal(result1, result3);
            Assert.Equal(result2, result4);
        }

        [Fact]
        public async Task Call_Async_Method_With_ExceptionHandler()
        {
            var mock = Substitute.For<IMockClass>();

            var mock1 = Substitute.For<IMockClass>();
            mock1.Method3Async().Returns("test");
            var mock2 = Substitute.For<IMockClass>();
            mock2.Method3Async().Returns(Task<string>.Factory.StartNew(() => throw new Exception()));

            await AlwaysResult.For<IMockClass>(mock1).InvokeAsync(c => c.Method3Async());
            await AlwaysResult.For<IMockClass>(mock2)
                .WithExceptionHandler((ex) => mock.Method1())
                .InvokeAsync(c => c.Method3Async());

            mock.Received().Method1();
        }                
    }

    public interface IMockClass
    {
        string Method1();

        string Method2(string a, int b);

        Task<string> Method3Async();

        Task<string> Method4Async(int a, int b);
    }
}
