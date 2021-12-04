using System;

namespace Codetox.Runtime.Pooling
{
    public abstract class ObjectPool<T> : IObjectPool<T>
    {
        protected readonly Action<T> ActionOnGet;
        protected readonly Action<T> ActionOnRelease;
        protected readonly Func<T> CreateFunc;
        protected readonly int Size;

        protected ObjectPool(Func<T> createFunc, Action<T> actionOnGet, Action<T> actionOnRelease, int size)
        {
            CreateFunc = createFunc;
            ActionOnGet = actionOnGet;
            ActionOnRelease = actionOnRelease;
            Size = size;
        }

        public virtual T Get()
        {
            throw new NotImplementedException();
        }

        public virtual void Return(T t)
        {
            throw new NotImplementedException();
        }
    }
}