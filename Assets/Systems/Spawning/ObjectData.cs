using UnityEngine;
namespace Spawning
{
    public class ObjectData<Id> : ScriptableObject
    {
        [field: SerializeField] public Id ID { get; protected set; }
    }
    public class MonoBehaviourData<Id, T> : ObjectData<Id> where T : ISpawnable<Id>
    {
        [field: SerializeField] public T Prefab { get; protected set; }
    }
}