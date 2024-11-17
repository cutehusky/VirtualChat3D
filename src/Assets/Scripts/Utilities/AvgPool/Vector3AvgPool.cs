using System.Linq;
using UnityEngine;

namespace Utilities.AvgPool
{
    /// <summary>
    /// Average Pooling for 3D vector
    /// </summary>
    public class Vector3AvgPool: AvgPool<Vector3>
    {
        public Vector3AvgPool(int cacheSize) : base(cacheSize)
        {}
        
        public override Vector3 Add(Vector3 newVal)
        {
            Cache[CurCacheIndex++] = newVal;
            CurCacheIndex %= CacheSize;
            return Avg = Cache.Aggregate(Vector3.zero, (current, vec) => current + vec) / CacheSize;
        }
    }
}