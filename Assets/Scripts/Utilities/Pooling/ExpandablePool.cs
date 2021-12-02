using System;

namespace Codetox.Runtime.Pooling
{
    public class ExpandablePool<T> : ListPool<T>
    {
        public ExpandablePool(Func<T> createFunc, Action<T> actionOnGet, Action<T> actionOnRelease, int size) : base(
            createFunc, actionOnGet, actionOnRelease, size)
        {
        }

        public override T Get()
        {
            var item = base.Get();
            if (!Equals(item, default(T))) return item;
            item = CreateFunc();
            Pool.Add(item);
            return item;
        }
    }
}