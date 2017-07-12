using System;

namespace Injected
{
    class LifecycleManagerFactory
    {
        public static ILifecycleManager<T> CreateLifecycleManager<T>(LifecycleType lifecycleType, Func<T> objectFactory) 
            where T : class
        {
            ILifecycleManager<T> lifecycleManager = null;

            switch (lifecycleType)
            {
                case LifecycleType.Transient:
                    lifecycleManager = new TransientLifecycleManager<T>(objectFactory);
                    break;
                case LifecycleType.Singleton:
                    lifecycleManager = new SingletonLifecycleManager<T>(objectFactory);
                    break;
                default:
                    throw new NotImplementedException($"Lifecycle type {lifecycleType} has not been implemented.");
            }

            return lifecycleManager;
        }
    }
}
