using Injected.Extended.Tests.TestClasses;
using System.Threading;
using Xunit;

namespace Injected.Extended.Tests
{
    public class ThreadStaticLifecycleManagerFactoryTests
    {
        public class CreateLifecycleManager
        {
            [Fact]
            public void CausesTheContainerToResolveToTheSameInstanceWhenCalledMultipletimes()
            {
                // arrange
                var container = new DependencyInjectionContainer();
                container.Register<A, A>(new ThreadStaticLifecycleManagerFactory<A>());

                // act
                var a1 = container.Resolve<A>();
                var a2 = container.Resolve<A>();

                // assert
                Assert.Equal(a1, a2);
            }

            [Fact]
            public void CausesTheContainerToResolveToDifferentInstancesWhenCalledFromDifferentThreads()
            {
                // arrange
                var container = new DependencyInjectionContainer();
                container.Register<A, A>(new ThreadStaticLifecycleManagerFactory<A>());

                A a1 = null;
                A a2 = null;

                // act
                var thread1 = new Thread(() => a1 = container.Resolve<A>());
                var thread2 = new Thread(() => a2 = container.Resolve<A>());

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
