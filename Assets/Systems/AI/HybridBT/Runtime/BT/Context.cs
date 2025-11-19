using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Context<T>
{
    public readonly Transform Transform;
    public readonly NavMeshAgent Agent;
    protected Dictionary<T, object> data = new();
    public Context(Transform transform)
    {
        Transform = transform;
        Agent = transform.GetComponent<NavMeshAgent>();
    }
    public void SetData<R>(T key, R value) => data[key] = value;
    public R GetData<R>(T key)
    {
        return data.TryGetValue(key, out var value) ? (R)value : default;
    }
}
