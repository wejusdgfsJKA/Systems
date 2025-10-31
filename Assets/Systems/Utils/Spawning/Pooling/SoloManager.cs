using System.Collections.Generic;
namespace Spawning.Pooling
{
    /// <summary>
    /// A manager which can only handle one type of object.
    /// </summary>
    public class SoloManager : Manager
    {
        /// <summary>
        /// Holds all the pooled objects.
        /// </summary>
        protected Stack<Poolable> pool = new();
        public override void ReturnToPool(Poolable poolable)
        {
            pool.Push(poolable);
        }
        protected override Poolable GetFromPool(SpawnableData objectData)
        {
            if (pool.TryPop(out Poolable result))
            {
                return result;
            }
            return null;
        }
    }
}