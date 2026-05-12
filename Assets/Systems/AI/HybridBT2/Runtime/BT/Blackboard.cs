using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace HybridBT2
{
    public class Blackboard
    {
        public readonly Transform Transform;
        public readonly NavMeshAgent Agent;
        public float DeltaTime { get; set; }
        protected Dictionary<int, object> data = new();
        public Blackboard(Transform transform)
        {
            Transform = transform;
            Agent = transform.GetComponent<NavMeshAgent>();
        }
        public void SetData<R>(int key, R value) => data[key] = value;
        public R GetData<R>(int key)
        {
            return data.TryGetValue(key, out var value) ? (R)value : default;
        }
    }
}