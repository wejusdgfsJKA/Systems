using UnityEngine;
using UnityEngine.Events;
namespace Spawning2
{
    public class Poolable : MonoBehaviour
    {
        protected ObjectPool.Keys poolKey;
        public ObjectPool.Keys PoolKey => poolKey;
        public UnityEvent Reset;
        public virtual void Initialize(PoolableData poolableData)
        {
            if (poolableData == null)
            {
                Debug.LogError($"PoolableData is null. Cannot initialize Poolable {gameObject.name}.");
                return;
            }
            poolKey = poolableData.PoolKey;
        }
        protected virtual void OnDisable()
        {
            ReturnToPool();
        }
        protected virtual void ReturnToPool()
        {
            gameObject.SetActive(false);
            ObjectPool.TryGetInstance().ReturnToPool(this);
        }
    }
}