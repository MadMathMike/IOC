using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Injected
{
    public class DependencyInjectionContainer
    {
        // TODO: Find a way to merge these two dictionaries
        private Dictionary<Type, ConstructorInfo> constructors = new Dictionary<Type, ConstructorInfo>();
        private Dictionary<Type, ILifecycleManager> lifecycleManagers = new Dictionary<Type, ILifecycleManager>();

        public void Register<TResolvable, TImplementation>(LifecycleType lifecycleType = LifecycleType.Transient)
            where TResolvable : class
            where TImplementation : TResolvable
        {
            var resolvableType = typeof(TResolvable);
            var implementationType = typeof(TImplementation);

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

            ILifecycleManager lifecycleManager = GetLifeCycleManager<TResolvable>(lifecycleType);

            this.constructors.Add(resolvableType, constructor);
            this.lifecycleManagers.Add(resolvableType, lifecycleManager);
        }

        public TResolvable Resolve<TResolvable>()
        {
            var typeToResolve = typeof(TResolvable);

            if (!this.lifecycleManagers.ContainsKey(typeToResolve))
                throw new TypeNotRegisteredException(typeToResolve);

            return (TResolvable)this.lifecycleManagers[typeToResolve].GetObject();
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

            // TODO: A possible future enhancement could include searching the whole dependency graph 
            // for unregistered types and throwing an exception that includes all of them
            if (unregisteredParameterType != null)
                throw new TypeNotRegisteredException(unregisteredParameterType);

            var arguments = constructorParameters.Select(p => this.lifecycleManagers[p.ParameterType].GetObject()).ToArray();

            return constructor.Invoke(arguments);
        }

        // TODO: Consider moving this method into a different class
        private ILifecycleManager GetLifeCycleManager<TResolvable>(LifecycleType lifecycleType) where TResolvable : class
        {
            ILifecycleManager lifecycleManager = null;

            Func<TResolvable> objectFactory = () => (TResolvable)this.Resolve(typeof(TResolvable));

            switch (lifecycleType)
            {
                case LifecycleType.Transient:
                    lifecycleManager = new TransientLifecycleManager<TResolvable>(objectFactory);
                    break;
                case LifecycleType.Singleton:
                    lifecycleManager = new SingletonLifecycleManager<TResolvable>(objectFactory);
                    break;
                default:
                    throw new NotImplementedException($"Lifecycle type {lifecycleType} has not been implemented.");
            }

            return lifecycleManager;
        }
    }
}
