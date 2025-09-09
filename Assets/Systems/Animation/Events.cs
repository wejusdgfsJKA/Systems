using EventBus;
using UnityEngine;
using UnityEngine.Events;
namespace Animation
{
    [System.Serializable]
    public struct CustomAnimEvent : IEvent
    {
        [field: SerializeField] public AnimEventKey ID { get; private set; }
    }
    [System.Serializable]
    public struct AnimEvent
    {
        public UnityEvent Event;
        [field: SerializeField] public AnimEventKey ID { get; private set; }
    }
}