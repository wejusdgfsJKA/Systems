using UnityEngine;
namespace Spawning.Pooling
{
    public class IDPoolable<Id> : Poolable
    {
        [field: SerializeField] public Id ID { get; protected set; }
    }
}