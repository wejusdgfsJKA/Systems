using System.Collections.Generic;
using UnityEngine;
namespace Utilities
{
    public static class ComponentRegister<T> where T : class
    {
        static readonly Dictionary<Transform, T> storage = new();
        public static T Get(Transform transform)
        {
            if (transform == null)
            {
                Debug.LogWarning($"Transform cannot be null when getting component of type {typeof(T)}.");
                return null;
            }
            return storage.TryGetValue(transform, out T instance) ? instance : null;
        }
        public static bool Register(Transform transform, T component)
        {
            if (transform == null)
            {
                Debug.LogError($"Transform cannot be null when registering component of type {typeof(T)}.");
                return false;
            }
            if (component == null)
            {
                Debug.LogError($"Component of type {typeof(T)} cannot be null when registering for transform {transform}.");
                return false;
            }
            return storage.TryAdd(transform, component);
        }
        public static void Unregister(Transform transform)
        {
            if (transform == null)
            {
                Debug.LogError($"Transform cannot be null when unregistering component of type {typeof(T)}.");
                return;
            }
            storage.Remove(transform);
        }
        public static void Clear()
        {
            storage.Clear();
        }
    }
}