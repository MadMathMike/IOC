namespace Injected
{
    public interface ILifecycleManager
    {
        object GetObject();
    }

    public interface ILifecycleManager<T> : ILifecycleManager
    {
        new T GetObject();
    }
}