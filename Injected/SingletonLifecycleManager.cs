using System;

namespace Injected
{
    class SingletonLifecycleManager<T> : ILifecycleManager<T> where T : class
    {
        private Func<T> factory;
        private T instance;
        private object threadLock = new object();

        public SingletonLifecycleManager(Func<T> factory)
        {
            this.factory = factory;
        }

        public T GetObject()
        {
            if (instance != null)
                return instance;

            lock (threadLock)
            {
                // A previous thread might have grabbed the lock first and set the instance field
                if (instance != null)
                    return instance;

                instance = this.factory();
            }

            return instance;
        }

        object ILifecycleManager.GetObject()
        {
            return this.GetObject();
        }
    }
}
