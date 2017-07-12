using System;
using System.Runtime.Serialization;

namespace Injected
{
    [Serializable]
    public class AbstractClassNotAllowedException : Exception
    {
        public readonly Type Type;

        public AbstractClassNotAllowedException()
        {
        }

        public AbstractClassNotAllowedException(Type type)
            : this(string.Format("Type {0} is abstract and cannot be used as an implementation type during registration.", type))
        {
            this.Type = type;
        }

        public AbstractClassNotAllowedException(string message) : base(message)
        {
        }

        public AbstractClassNotAllowedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AbstractClassNotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}