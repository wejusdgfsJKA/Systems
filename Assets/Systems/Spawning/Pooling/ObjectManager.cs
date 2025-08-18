using Spawning;
using UnityEngine;
namespace Pooling
{
    public abstract class ObjectManager<Id> : MonoBehaviour
    {
        protected MultiPool<Id, Poolable<Id>> multiPool = new();
        protected ISpawner<Id, Poolable<Id>> spawner;
        public static ObjectManager<Id> Instance { get; protected set; }
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        public Poolable<Id> Get(ObjectData<Id> data)
        {
            Poolable<Id> t;
            if (multiPool.TryGet(data.ID, out t))
            {
                t.ResetObject();
                return t;
            }
            return spawner.Create(data);
        }
        public void Release(Poolable<Id> @object)
        {
            multiPool.Release(@object);
        }
    }
}