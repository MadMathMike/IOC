using System;

namespace Injected
{
    class TransientLifecycleManager<T> : ILifecycleManager<T> where T : class
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
