using BT;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
namespace UtilityAI
{
    [System.Serializable]
    public class Context : BlackBoard<ContextDataKeys>
    {
        public Transform Transform { get; protected set; }
        public NavMeshAgent Agent { get; protected set; }
        [field: SerializeField] public float RegularSpeed { get; protected set; } = 3.5f;
        [field: SerializeField] public float ChaseSpeed { get; protected set; } = 3.5f;
        [SerializeField] protected ContextEvent[] events;
        protected readonly Dictionary<ContextEventKeys, UnityEvent> eventDictionary = new();
        public void Initialize(Transform transform)
        {
            Transform = transform;
            Agent = transform.GetComponent<NavMeshAgent>();
            if (events != null)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    eventDictionary.Add(events[i].Key, events[i].Event);
                }
            }
        }
        public bool InvokeEvent(ContextEventKeys key)
        {
            UnityEvent @event;
            if (eventDictionary.TryGetValue(key, out @event))
            {
                @event?.Invoke();
            }
            return false;
        }
    }
}