using Injected.Tests.TestClasses;
using System;
using System.Threading;
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

            // This test is problematic since one thing this tests is a race condition. 
            // I did get the test to fail a few times before I added my code for thread 
            // safety in the SingletonLifecycleManager though.
            [Fact]
            public void ReturnsTheSameInstanceEvenWhenCalledFromSeparateThreads()
            {
                // arrange
                Func<A> factory = () => new A();
                var singletonLifecycleManager = new SingletonLifecycleManager<A>(factory);

                A a1 = null;
                A a2 = null;

                // act
                var thread1 = new Thread(() => a1 = singletonLifecycleManager.GetObject());
                var thread2 = new Thread(() => a2 = singletonLifecycleManager.GetObject());

                thread1.Start();
                thread2.Start();

                thread1.Join();
                thread2.Join();

                // assert
                Assert.NotNull(a1);
                Assert.NotNull(a2);
                Assert.Equal(a1, a2);
            }
        }
    }
}
