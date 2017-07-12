using Injected.Tests.TestClasses;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Injected.Tests
{
    public class SingletonLifecycleManagerTests
    {
        public class GetObject
        {
            [Fact]
            public void ReturnsTheSameInstanceEvenWhenCalledMultipleTimes()
            {
                // arrange
                Func<A> factory = () => new A();
                var singletonLifecycleManager = new SingletonLifecycleManager<A>(factory);

                // act
                var a1 = singletonLifecycleManager.GetObject();
                var a2 = singletonLifecycleManager.GetObject();

                // assert
                Assert.NotNull(a1);
                Assert.NotNull(a2);
                Assert.Equal(a1, a2);
            }

            // This test is problematic since I'm basically trying to test a race condition. 
            // I did get the test to fail a few times before I added my code for thread 
            // safety in the SingletonLifecycleManager though.
            [Fact]
            public async Task ReturnsTheSameInstanceEvenWhenCalledFromSeparateThreads()
            {
                // arrange
                Func<A> factory = () => new A();
                var singletonLifecycleManager = new SingletonLifecycleManager<A>(factory);
                
                // act
                var task1 = Task.Run<A>(() => singletonLifecycleManager.GetObject());
                var task2 = Task.Run<A>(() => singletonLifecycleManager.GetObject());

                var a1 = await task1;
                var a2 = await task2;

                // assert
                Assert.NotNull(a1);
                Assert.NotNull(a2);
                Assert.Equal(a1, a2);
            }
        }
    }
}
