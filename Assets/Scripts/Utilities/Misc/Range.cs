using System;

namespace Utilities
{
    [Serializable]
    public struct Range<T>
    {
        public T min, max;
    }
}