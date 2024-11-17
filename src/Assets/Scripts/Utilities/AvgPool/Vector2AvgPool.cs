using System.Linq;
using UnityEngine;

namespace Utilities.AvgPool
{
    /// <summary>
    /// Average Pooling for 2D vector
    /// </summary>
    public class Vector2AvgPool: AvgPool<Vector2>
    {
        public Vector2AvgPool(int cacheSize) : base(cacheSize)
        {
        }

        public override Vector2 Add(Vector2 newVal)
        {
            Cache[CurCacheIndex++] = newVal;
            CurCacheIndex %= CacheSize;
            return Avg = Cache.Aggregate(Vector2.zero, (current, vec) => current + vec) / CacheSize;
        }
    }
}