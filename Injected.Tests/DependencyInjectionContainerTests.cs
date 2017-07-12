using Injected.Tests.TestClasses;
using System;
using Xunit;

namespace Injected.Tests
{
    public class DependencyInjectionContainerTests
    {
        [Fact]
        public void CanResolveClasses()
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
        public void CanResolveInterfaces()
        {
            // arrange
            var container = new DependencyInjectionContainer();
            container.Register<IA, A>();

            // act
            var a = container.Resolve<IA>();

            // assert
            Assert.NotNull(a);
        }

        [Fact]
        public void CanResolveClassesWithDependencies()
        {
            // arrange
            var container = new DependencyInjectionContainer();
            container.Register<IA, A>();
            container.Register<B, B>();

            // act
            var b = container.Resolve<B>();

            // assert
            Assert.NotNull(b);
        }
        
        [Fact]
        public void CanResolveGenericClass()
        {
            // arrange
            var container = new DependencyInjectionContainer();
            container.Register<GenericClass<IA>, GenericClass<IA>>();
            container.Register<IA, A>();

            // act
            var generic = container.Resolve<GenericClass<IA>>();

            // assert
            Assert.NotNull(generic);
        }

        [Fact]
        public void DefaultsToTransientLifetimes()
        {
            // arrange
            var container = new DependencyInjectionContainer();
            container.Register<A, A>();

            // act
            var a1 = container.Resolve<A>();
            var a2 = container.Resolve<A>();

            // assert
            Assert.NotNull(a1);
            Assert.NotNull(a2);
            Assert.NotEqual(a1, a2);
        }

        [Fact]
        public void ResolvesToTheSameInstanceForSingletonRegistrations()
        {
            // arrange
            var container = new DependencyInjectionContainer();
            container.Register<A, A>(LifecycleType.Singleton);

            // act
            var a1 = container.Resolve<A>();
            var a2 = container.Resolve<A>();

            // assert
            Assert.NotNull(a1);
            Assert.NotNull(a2);
            Assert.Equal(a1, a2);
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
                Assert.True(false, $"Type {typeof(A)} should not be registered, so we should not be able to resolve it from the container.");
            }
            catch (TypeNotRegisteredException)
            {
                // Do nothing because this is exactly what we wanted.
            }
        }

        [Fact]
        public void ThrowsExceptionWhenResolvingTypeWithUnregisteredDependency()
        {
            // arrange
            var container = new DependencyInjectionContainer();
            container.Register<B, B>();

            // act
            try
            {
                var b = container.Resolve<B>();
                Assert.True(false, $"Type {typeof(B)} has an unregistered dependency, so we should not be able to resolve it from the container.");
            }
            catch (TypeNotRegisteredException)
            {
                // Do nothing because this is exactly what we wanted.
            }
        }

        [Fact]
        public void ThrowsExceptionWhenResolvingUnregisteredGenericType()
        {
            // arrange
            var container = new DependencyInjectionContainer();
            container.Register<GenericClass<A>, GenericClass<A>>();
            container.Register<A, A>();
            
            // act
            try
            {
                // act
                var generic = container.Resolve<GenericClass<B>>();
                Assert.True(false, $"Type {typeof(GenericClass<B>)} should not be registered, so we should not be able to resolve it from the container.");
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

        [Fact]
        public void ThrowsExceptionWhenRegisteringAbstractClass()
        {
            // arrange
            var container = new DependencyInjectionContainer();

            // act
            try
            {
                container.Register<AbstractClass, AbstractClass>();
                Assert.True(false, $"Type {typeof(AbstractClass)} should not successfully register because it is abstract.");
            }
            catch (AbstractClassNotAllowedException)
            {
                // Do nothing because this is exactly what we wanted.
            }
        }
    }
}
