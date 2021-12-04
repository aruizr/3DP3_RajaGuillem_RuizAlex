using System;
using System.Collections.Generic;
using System.Linq;

namespace Codetox.Runtime.Pooling
{
    public class ListPool<T> : ObjectPool<T>
    {
        protected readonly List<T> Pool;

        public ListPool(Func<T> createFunc, Action<T> actionOnGet, Action<T> actionOnRelease, int size) : base(
            createFunc, actionOnGet, actionOnRelease, size)
        {
            Pool = new List<T>();
            for (var i = 0; i < Size; i++) Pool.Add(CreateFunc());
        }

        public override T Get()
        {
            var item = Pool.FirstOrDefault();
            if (item == null) return default;
            Pool.Remove(item);
            ActionOnGet?.Invoke(item);
            return item;
        }

        public override void Return(T t)
        {
            ActionOnRelease?.Invoke(t);
            Pool.Add(t);
        }
    }
}