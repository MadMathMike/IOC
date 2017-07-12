using Injected.Tests.TestClasses;
using System;
using Xunit;

namespace Injected.Tests
{
    public class TransientLifecycleManagerTests
    {
        public class GetObject
        {
            [Fact]
            public void ReturnsTheResultOfTheFactoryMethodEachTimeItIsCalled()
            {
                // arrange
                Func<A> factory = () => new A();
                var transientLifecycleManager = new TransientLifecycleManager<A>(factory);

                // act
                var a1 = transientLifecycleManager.GetObject();
                var a2 = transientLifecycleManager.GetObject();

                // assert
                Assert.NotNull(a1);
                Assert.NotNull(a2);
                Assert.NotEqual(a1, a2);
            }
        }
    }
}
