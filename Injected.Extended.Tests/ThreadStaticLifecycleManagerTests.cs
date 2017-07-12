using Injected.Extended.Tests.TestClasses;
using System;
using System.Threading;
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
            public void ReturnsDifferentObjectsWhenCalledOnDifferentThreads()
            {
                // arrange
                Func<A> factory = () => new A();
                var threadStaticLifecycleManager = new ThreadStaticLifecycleManager<A>(factory);

                A a1 = null;
                A a2 = null;

                // act
                var thread1 = new Thread(() => a1 = threadStaticLifecycleManager.GetObject());
                var thread2 = new Thread(() => a2 = threadStaticLifecycleManager.GetObject());

                thread1.Start();
                thread2.Start();

                thread1.Join();
                thread2.Join();

                // assert
                Assert.NotEqual(a1, a2);
            }
        }
    }
}
