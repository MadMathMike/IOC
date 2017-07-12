using System;

namespace Injected.Extended
{
    public class ThreadStaticLifecycleManager<T> : ILifecycleManager<T> where T : class
    {
        private Func<T> factory;

        [ThreadStatic]
        private static T instance;

        public ThreadStaticLifecycleManager(Func<T> factory)
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