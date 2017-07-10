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

        [Fact]
        public void ThrowsExceptionWhenResolvingUnregisteredType()
        {
            // arrange
            var container = new DependencyInjectionContainer();

            // act
            try
            {
                var a = container.Resolve<A>();
                Assert.True(false, $"Type {a} should not be registered, so we should not be able to resolve it from the container.");
            }
            catch (TypeNotRegisteredException)
            {
                // Do nothing because this is exactly what we wanted.
            }
        }

        [Fact]
        public void ThrowsExceptionWhenRegisteringTypeWithNoPublicConstructors()
        {
            // arrange
            var container = new DependencyInjectionContainer();

            // act
            try
            {
                container.Register<NoPublicConstructors, NoPublicConstructors>();
                Assert.True(false, $"Type {typeof(NoPublicConstructors)} should not successfully register because it has no public constructors.");
            }
            catch (NoPublicConstructorsException)
            {
                // Do nothing because this is exactly what we wanted.
            }
        }
        
        [Fact]
        public void ThrowsExceptionWhenRegisteringTypeWithMoreThanOnePublicConstructor()
        {
            // arrange
            var container = new DependencyInjectionContainer();

            // act
            try
            {
                container.Register<TwoPublicConstructors, TwoPublicConstructors>();
                Assert.True(false, $"Type {typeof(TwoPublicConstructors)} should not successfully register because it has more than one public constructor.");
            }
            catch (MoreThanOnePublicConstructorException)
            {
                // Do nothing because this is exactly what we wanted.
            }
        }
    }
}
