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
        private Dictionary<Type, ConstructorInfo> constructors = new Dictionary<Type, ConstructorInfo>();

        public void Register<TResolvable, TImplementation>()
            where TResolvable : class
            where TImplementation : TResolvable
        {
            var implementationType = typeof(TImplementation);
            
            var constructors = implementationType.GetConstructors();

            // Design assumption: Implementation classes must have only one public constructor. 
            // My gut says that if there is more than one public constructor, then the dependencies in the class haven't been made very clear.

            if (constructors.Length == 0)
                throw new NoPublicConstructorsException(implementationType);

            if (constructors.Length > 1)
                throw new MoreThanOnePublicConstructorException(implementationType);

            var constructor = constructors.Single();

            this.constructors.Add(typeof(TResolvable), constructor);
        }

        public TResolvable Resolve<TResolvable>()
        {
            var typeToResolve = typeof(TResolvable);

            return (TResolvable)Resolve(typeToResolve);
        }

        private object Resolve(Type typeToResolve)
        {
            if (!constructors.ContainsKey(typeToResolve))
                throw new TypeNotRegisteredException(typeToResolve);

            var constructor = constructors[typeToResolve];
            var arguments = constructor.GetParameters().Select(p => this.Resolve(p.ParameterType)).ToArray();

            return constructor.Invoke(arguments);
        } 
    }
}
