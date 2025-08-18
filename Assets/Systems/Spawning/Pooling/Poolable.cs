using Pooling;
using Spawning;
using UnityEngine;

public class Poolable<Id> : MonoBehaviour, IPoolable<Id>
{
    [SerializeField] protected Id id;
    public Id ID
    {
        get
        {
            return id;
        }
    }
    public virtual void Init(ObjectData<Id> data)
    {
        id = data.ID;
    }

    /// <summary>
    /// Reset this poolable object. This is called BEFORE enabling it!
    /// </summary>
    public virtual void ResetObject() { }
    protected virtual void OnDisable()
    {
        ObjectManager<Id>.Instance.Release(this);
    }
}
