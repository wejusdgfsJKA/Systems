using System.Collections.Generic;
using UnityEngine;

public class Context<T>
{
    public readonly Transform Transform;
    protected Dictionary<T, object> data = new();
    public Context(Transform transform)
    {
        Transform = transform;
    }
    public void SetValue<R>(T key, R value) => data[key] = value;
    public R GetValue<R>(T key)
    {
        return data.TryGetValue(key, out var value) ? (R)value : default;
    }
}
