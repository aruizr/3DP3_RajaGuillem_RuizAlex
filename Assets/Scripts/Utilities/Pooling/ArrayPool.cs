using System;

namespace Codetox.Runtime.Pooling
{
    public class ArrayPool<T> : ObjectPool<T>
    {
        private readonly PoolItem[] _pool;

        public ArrayPool(Func<T> createFunc, Action<T> actionOnGet, Action<T> actionOnRelease, int size) : base(
            createFunc, actionOnGet, actionOnRelease, size)
        {
            _pool = new PoolItem[Size];
            for (var i = 0; i < Size; i++) _pool[i] = new PoolItem(CreateFunc(), true);
        }

        public override T Get()
        {
            foreach (var poolItem in _pool)
            {
                if (!poolItem.InPool) continue;
                var t = poolItem.Item;
                ActionOnGet?.Invoke(t);
                poolItem.InPool = false;
                return t;
            }

            return default;
        }

        public override void Return(T t)
        {
            foreach (var poolItem in _pool)
            {
                if (!poolItem.Item.Equals(t)) continue;
                ActionOnRelease?.Invoke(t);
                poolItem.InPool = true;
                break;
            }
        }

        private class PoolItem
        {
            public readonly T Item;
            public bool InPool;

            public PoolItem(T item, bool inPool)
            {
                Item = item;
                InPool = inPool;
            }
        }
    }
}