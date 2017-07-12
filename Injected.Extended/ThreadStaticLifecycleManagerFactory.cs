using System;

namespace Injected.Extended
{
    public class ThreadStaticLifecycleManagerFactory<T> : ILifecycleManagerFactory<T> where T : class
    {
        public ILifecycleManager<T> CreateLifecycleManager(Func<T> objectFactory)
        {
            return new ThreadStaticLifecycleManager<T>(objectFactory);
        }
    }
}
