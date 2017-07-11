using System;

namespace Injected
{
    class SingletonLifecycleManager<T> : ILifecycleManager<T> where T : class
    {
        private Func<T> factory;
        private T instance;

        public SingletonLifecycleManager(Func<T> factory)
        {
            this.factory = factory;
        }

        public T GetObject()
        {
            return instance = instance ?? this.factory();
        }

        object ILifecycleManager.GetObject()
        {
            return this.GetObject();
        }
    }
}
