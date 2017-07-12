using System;

namespace Injected
{
    public interface ILifecycleManagerFactory<T> where T : class
    {
        ILifecycleManager<T> CreateLifecycleManager(Func<T> objectFactory);
    }
}
