using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Injected
{
    public class DependencyInjectionContainer
    {
        // I don't think it is necessary to try to make these dictionaries thread safe.
        // From what I've seen of IOC container usage, registration usually happens up front in a single thread, 
        // so adding to these dictionaries should be safe. Also, because each of the lifecycle managers has to be 
        // thread safe, I don't think it matters that multiple threads try to read from the dictionary at the same time.
        private Dictionary<Type, ConstructorInfo> constructors = new Dictionary<Type, ConstructorInfo>();
        private Dictionary<Type, ILifecycleManager> lifecycleManagers = new Dictionary<Type, ILifecycleManager>();

        public void Register<TResolvable, TImplementation>(LifecycleType lifecycleType = LifecycleType.Transient)
            where TResolvable : class
            where TImplementation : TResolvable
        {
            var resolvableType = typeof(TResolvable);
            var implementationType = typeof(TImplementation);
            Func<TResolvable> objectFactory = () => (TResolvable)this.Resolve(resolvableType);

            // This static dependency is most useful in that I can now test the creation of the lifecycle managers separately.
            // An argument could be made that this is both more and less SOLID: it helps adhere to SRP, but 
            // it violates dependency inversion (which is a bit ironic given what I'm writing).
            ILifecycleManager lifecycleManager = LifecycleManagerFactory.CreateLifecycleManager(lifecycleType, objectFactory);
            RegisterTypes(resolvableType, implementationType, lifecycleManager);
        }

        public void Register<TResolvable, TImplementation>(ILifecycleManagerFactory<TResolvable> lifecycleManagerFactory)
            where TResolvable : class
            where TImplementation : TResolvable
        {
            var resolvableType = typeof(TResolvable);
            var implementationType = typeof(TImplementation);
            Func<TResolvable> objectFactory = () => (TResolvable)this.Resolve(resolvableType);

            ILifecycleManager lifecycleManager = lifecycleManagerFactory.CreateLifecycleManager(objectFactory);
            RegisterTypes(resolvableType, implementationType, lifecycleManager);
        }

        public TResolvable Resolve<TResolvable>()
        {
            var typeToResolve = typeof(TResolvable);

            if (!this.lifecycleManagers.ContainsKey(typeToResolve))
                throw new TypeNotRegisteredException(typeToResolve);

            return (TResolvable)this.lifecycleManagers[typeToResolve].GetObject();
        }

        private void RegisterTypes(Type resolvableType, Type implementationType, ILifecycleManager lifecycleManager)
        {
            if (implementationType.IsAbstract)
                throw new AbstractClassNotAllowedException(implementationType);

            // Design assumption: Implementation classes must have only one public constructor. 
            // My gut says that if there is more than one public constructor, then the dependencies in the class haven't been made very clear.
            // Side benefit of checking constructors: it ensures the implmentation type is actually a class (instead of an interface or a delegate, for instance).
            var constructors = implementationType.GetConstructors();

            if (constructors.Length == 0)
                throw new NoPublicConstructorsException(implementationType);

            if (constructors.Length > 1)
                throw new MoreThanOnePublicConstructorException(implementationType);

            var constructor = constructors.Single();

            this.constructors.Add(resolvableType, constructor);
            this.lifecycleManagers.Add(resolvableType, lifecycleManager);
        }

        private object Resolve(Type typeToResolve)
        {
            if (!this.constructors.ContainsKey(typeToResolve))
                throw new TypeNotRegisteredException(typeToResolve);

            var constructor = constructors[typeToResolve];
            var constructorParameters = constructor.GetParameters();

            var unregisteredParameterType = constructorParameters
                                                    .Where(p => !this.lifecycleManagers.ContainsKey(p.ParameterType))
                                                    .Select(p => p.ParameterType)
                                                    .FirstOrDefault();

            // A future enhancement could include searching the whole dependency graph 
            // for unregistered types and throwing an exception that includes all of them
            if (unregisteredParameterType != null)
                throw new TypeNotRegisteredException(unregisteredParameterType);

            var arguments = constructorParameters.Select(p => this.lifecycleManagers[p.ParameterType].GetObject()).ToArray();

            return constructor.Invoke(arguments);
        }
    }
}
