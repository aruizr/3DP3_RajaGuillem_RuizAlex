namespace Codetox.Runtime.Pooling
{
    public interface IObjectPool<T>
    {
        T Get();
        void Return(T t);
    }
}