using UnityEngine;
namespace Spawning2
{
    [CreateAssetMenu(menuName = "Spawning2/PoolableData")]
    public class PoolableData : ScriptableObject
    {
        public ObjectPool.Keys PoolKey;
        public Poolable Prefab;
    }
}