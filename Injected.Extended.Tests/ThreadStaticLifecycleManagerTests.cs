using Injected.Extended.Tests.TestClasses;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Injected.Extended.Tests
{
    public class ThreadStaticLifecycleManagerTests
    {
        public class GetObject
        {
            [Fact]
            public void ReturnsTheSameObjectWhenCalledOnTheSameThread()
            {
                // arrange
                Func<A> factory = () => new A();
                var threadStaticLifecycleManager = new ThreadStaticLifecycleManager<A>(factory);

                // act
                var a1 = threadStaticLifecycleManager.GetObject();
                var a2 = threadStaticLifecycleManager.GetObject();

                // assert
                Assert.Equal(a1, a2);
            }

            [Fact]
            public async Task ReturnsDifferentObjectsWhenCalledOnDifferentThreads()
            {
                // arrange
                Func<A> factory = () => new A();
                var threadStaticLifecycleManager = new ThreadStaticLifecycleManager<A>(factory);

                // act
                var task1 = Task.Run(() => threadStaticLifecycleManager.GetObject());
                var task2 = Task.Run(() => threadStaticLifecycleManager.GetObject());
                var a1 = await task1;
                var a2 = await task2;

                // assert
                Assert.NotEqual(a1, a2);
            }
        }
    }
}
