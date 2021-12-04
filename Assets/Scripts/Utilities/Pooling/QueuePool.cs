using System;
using System.Collections.Generic;

namespace Codetox.Runtime.Pooling
{
    public class QueuePool<T> : ObjectPool<T>
    {
        private readonly Queue<T> _pool;

        public QueuePool(Func<T> createFunc, Action<T> actionOnGet, Action<T> actionOnRelease, int size) : base(
            createFunc, actionOnGet, actionOnRelease, size)
        {
            _pool = new Queue<T>();
            for (var i = 0; i < Size; i++) _pool.Enqueue(CreateFunc());
        }

        public override T Get()
        {
            var t = _pool.Dequeue();
            ActionOnGet?.Invoke(t);
            return t;
        }

        public override void Return(T t)
        {
            ActionOnRelease?.Invoke(t);
            _pool.Enqueue(t);
        }
    }
}