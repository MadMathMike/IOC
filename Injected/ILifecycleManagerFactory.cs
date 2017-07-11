using System;

namespace Injected
{
    interface ILifecycleManagerFactory
    {
        ILifecycleManager<T> CreateLifecycleManager<T>(Func<T> factory);
    }
}
