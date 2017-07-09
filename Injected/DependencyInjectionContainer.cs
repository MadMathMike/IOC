using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Injected
{
    public class DependencyInjectionContainer
    {
        public void Register<TResolvable, TImplementation>()
            where TResolvable : class
            where TImplementation : TResolvable
        {
            throw new NotImplementedException();
        }

        public TResolvable Resolve<TResolvable>()
        {
            throw new NotImplementedException();
        }
    }
}
