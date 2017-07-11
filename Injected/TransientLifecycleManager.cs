using System;

namespace Injected
{
    class TransientLifecycleManager<T> : ILifecycleManager<T>
    {
        private Func<T> factory;

        public TransientLifecycleManager(Func<T> factory)
        {   
            this.factory = factory;
        }

        public T GetObject()
        {
            return this.factory();
        }

        object ILifecycleManager.GetObject()
        {
            return this.GetObject();
        }
    }
}
