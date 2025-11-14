using EventBus;
using Tags;
using UnityEngine;
namespace Effects
{
    public class HealArea : Taggable<AreaType>
    {
        [SerializeField] int healAmount = 1;
        ReceiveHealOverTime @event;
        RemoveEffect removeEffect;
        private void Awake()
        {
            currentTags ??= new() { AreaType.HealArea };
            @event = new(transform.GetInstanceID(), float.PositiveInfinity, 1, healAmount);
            removeEffect = new(transform.GetInstanceID());
        }
        private void OnTriggerEnter(Collider other)
        {
            EventBus<ReceiveHealOverTime>.Raise(other.transform.root.GetInstanceID(), @event);
        }
        private void OnTriggerExit(Collider other)
        {
            EventBus<RemoveEffect>.Raise(other.transform.root.GetInstanceID(), removeEffect);
        }
    }
}