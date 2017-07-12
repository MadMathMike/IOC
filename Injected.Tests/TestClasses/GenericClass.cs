using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Injected.Tests.TestClasses
{
    class GenericClass<T>
    {
        public GenericClass(T property)
        {
            this.Property = property;
        }

        public T Property { get; set; }
    }
}
