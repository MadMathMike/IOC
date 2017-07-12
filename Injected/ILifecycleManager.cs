namespace Injected
{
    public interface ILifecycleManager
    {
        object GetObject();
    }

    public interface ILifecycleManager<T> : ILifecycleManager where T : class
    {
        new T GetObject();
    }
}