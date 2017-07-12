using System;
using System.Runtime.Serialization;

namespace Injected
{
    [Serializable]
    public class NoPublicConstructorsException : Exception
    {
        public readonly Type Type;

        public NoPublicConstructorsException()
        {
        }

        public NoPublicConstructorsException(Type type)
            : this(string.Format("Type {0} contains no public constructors.", type))
        {
            this.Type = type;            
        }

        public NoPublicConstructorsException(string message) : base(message)
        {
        }

        public NoPublicConstructorsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoPublicConstructorsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}