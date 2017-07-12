using Injected.Tests.TestClasses;
using System;
using Xunit;

namespace Injected.Tests
{
    public class LifecycleManagerFactoryTests
    {
        public class CreateLifecycleManager
        {
            [Theory]
            [InlineData(LifecycleType.Transient, typeof(TransientLifecycleManager<A>))]
            [InlineData(LifecycleType.Singleton, typeof(SingletonLifecycleManager<A>))]
            public void ReturnsCorrectLifecycleManager(LifecycleType lifecycleType, Type typeOfLifecycleManager)
            {
                // arrange
                Func<A> objectFactory = () => new A();

                // act
                var lifecycleManager = LifecycleManagerFactory.CreateLifecycleManager(lifecycleType, objectFactory);

                // assert
                Assert.IsType(typeOfLifecycleManager, lifecycleManager);
            }
        }
    }
}
