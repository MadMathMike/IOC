using System;
using System.Runtime.Serialization;

namespace Injected
{
    [Serializable]
    public class TypeNotRegisteredException : Exception
    {
        public readonly Type Type;

        public TypeNotRegisteredException()
        {
        }

        public TypeNotRegisteredException(Type type)
            :this(string.Format("Type {0} is not registered in the container.", type))
        {
            this.Type = type;
        }

        public TypeNotRegisteredException(string message) : base(message)
        {
        }

        public TypeNotRegisteredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TypeNotRegisteredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}