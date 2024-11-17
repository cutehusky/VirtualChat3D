
namespace Utilities.AvgPool
{
    /// <summary>
    /// Average Pooling template
    /// </summary>
    /// <typeparam name="T"> type of value </typeparam>
    public abstract class AvgPool<T>
    {
        protected readonly int CacheSize;
        protected T[] Cache;
        protected int CurCacheIndex;
        
        /// <summary>
        /// Average value of pool
        /// </summary>
        public T Avg;
        protected AvgPool(int cacheSize)
        {
            CacheSize = cacheSize;
            Cache = new T[CacheSize];
        }

        public void Clear()
        {
            Cache = new T[CacheSize];
            Avg = default;
        }
        
        /// <summary>
        /// Add new value to pool and return average value of pool
        /// </summary>
        /// <param name="newVal"> new value </param>
        /// <returns></returns>
        public abstract T Add(T newVal);
    }
}