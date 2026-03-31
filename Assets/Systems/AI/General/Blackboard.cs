using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace AIStuff
{
    public class Blackboard
    {
        public Transform Transform { get; }
        protected Dictionary<int, object> data = new();
        public Blackboard(Transform transform)
        {
            Transform = transform;
        }
        public void SetData<R>(int key, R value) => data[key] = value;
        public R GetData<R>(int key)
        {
            return data.TryGetValue(key, out var value) ? (R)value : default;
        }
    }
    public class NavAgentBlackboard : Blackboard
    {
        public NavMeshAgent Agent { get; }
        public NavAgentBlackboard(Transform transform) : base(transform)
        {
            Agent = transform.GetComponent<NavMeshAgent>();
        }
    }
}