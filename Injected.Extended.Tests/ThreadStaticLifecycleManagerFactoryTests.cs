using Injected.Extended.Tests.TestClasses;
using System.Threading.Tasks;
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
            public async Task CausesTheContainerToResolveToDifferentInstancesWhenCalledFromDifferentThreads()
            {
                // arrange
                var container = new DependencyInjectionContainer();
                container.Register<A, A>(new ThreadStaticLifecycleManagerFactory<A>());

                // act
                var task1 = Task.Run(() => container.Resolve<A>());
                var task2 = Task.Run(() => container.Resolve<A>());
                var a1 = await task1;
                var a2 = await task2;

                // assert
                Assert.NotEqual(a1, a2);
            }
        }
    }
}
