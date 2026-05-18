using System.Collections.Generic;
using Utilities;
namespace Spawning2
{
    public class ObjectPool : Singleton<ObjectPool>
    {
        public enum Keys
        {

        }
        protected Dictionary<Keys, Stack<Poolable>> pools = new();
        public Poolable Spawn(PoolableData data)
        {
            if (!pools.TryGetValue(data.PoolKey, out var pool))
            {
                pools[data.PoolKey] = pool = new Stack<Poolable>();
            }
            if (pool.Count > 0)
            {
                var obj = pool.Pop();
                obj.Initialize(data);
                obj.Reset?.Invoke();
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                return Create(data);
            }
        }
        protected Poolable Create(PoolableData data)
        {
            var obj = Instantiate(data.Prefab);
            obj.Initialize(data);
            return obj;
        }
        public void ReturnToPool(Poolable poolable)
        {
            poolable.gameObject.SetActive(false);
            if (!pools.TryGetValue(poolable.PoolKey, out var pool))
            {
                pools[poolable.PoolKey] = pool = new Stack<Poolable>();
            }
            pool.Push(poolable);
        }
        protected override void ClearSingleton()
        {
            base.ClearSingleton();
            pools.Clear();
        }
    }
}