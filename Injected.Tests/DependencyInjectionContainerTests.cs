using Injected.Tests.TestClasses;
using System;
using Xunit;

namespace Injected.Tests
{
    public class DependencyInjectionContainerTests
    {
        [Fact]
        public void ClassACanBeResolved()
        {
            // arrange
            var container = new DependencyInjectionContainer();
            container.Register<A, A>();

            // act
            var a = container.Resolve<A>();

            // assert
            Assert.NotNull(a);
        }
    }
}
