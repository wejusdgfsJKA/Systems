using System.Collections.Generic;
using UnityEngine;
namespace Utilities
{
    public static class ComponentRegister<T> where T : Component
    {
        static Dictionary<Transform, T> storage = new();
        public static T Get(Transform transform)
        {
            return storage.TryGetValue(transform, out T instance) ? instance : null;
        }
        public static bool Register(Transform transform, T component)
        {
            return storage.TryAdd(transform, component);
        }
        public static void Unregister(Transform transform)
        {
            storage.Remove(transform);
        }
    }
}