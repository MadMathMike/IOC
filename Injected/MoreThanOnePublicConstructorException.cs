using System;
using System.Runtime.Serialization;

namespace Injected
{
    [Serializable]
    public class MoreThanOnePublicConstructorException : Exception
    {
        public readonly Type Type;

        public MoreThanOnePublicConstructorException()
        {
        }

        public MoreThanOnePublicConstructorException(Type type)
            : this(string.Format("Type {0} contains more than one public constructor.", type))
        {
            this.Type = type;
        }

        public MoreThanOnePublicConstructorException(string message) : base(message)
        {
        }

        public MoreThanOnePublicConstructorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MoreThanOnePublicConstructorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}