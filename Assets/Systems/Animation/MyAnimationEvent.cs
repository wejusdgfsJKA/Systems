using EventBus;
using UnityEngine;
namespace Animation
{
    [System.Serializable]
    public struct MyAnimationEvent : IEvent
    {
        [field: SerializeField] public string Name { get; private set; }
    }
}