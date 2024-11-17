using System.Linq;

namespace Utilities.AvgPool
{
    /// <summary>
    /// Average Pooling of 1D float
    /// </summary>
    public class FloatAvgPool: AvgPool<float>
    {
        public FloatAvgPool(int cacheSize) : base(cacheSize)
        { }

        public override float Add(float newVal)
        {
            Cache[CurCacheIndex++] = newVal;
            CurCacheIndex %= CacheSize;
            return Avg = Cache.Aggregate(0f, (current, v) => current + v) / CacheSize;
        }
    }
}