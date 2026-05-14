using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace HybridBT2
{
    public class Blackboard
    {
        public enum Keys
        {

        }
        public readonly Transform Transform;
        public readonly NavMeshAgent Agent;
        public float DeltaTime { get; set; }
        protected Dictionary<Keys, object> data = new();
        public Blackboard(Transform transform)
        {
            Transform = transform;
            Agent = transform.GetComponent<NavMeshAgent>();
        }
        public void SetData<R>(Keys key, R value) => data[key] = value;
        public R GetData<R>(Keys key)
        {
            return data.TryGetValue(key, out var value) ? (R)value : default;
        }
    }
}