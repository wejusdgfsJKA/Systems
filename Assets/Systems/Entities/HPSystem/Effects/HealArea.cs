using HP;
using Tags;
using UnityEngine;
namespace Effects
{
    public class HealArea : Taggable<AreaType>
    {
        [SerializeField] int healAmount = 1;
        ReceiveHealOverTime receiveHeal;
        RemoveEffect removeHeal;
        private void Awake()
        {
            currentTags ??= new() { AreaType.HealArea };
            receiveHeal = new(transform.GetInstanceID(), float.PositiveInfinity, 1, healAmount);
            removeHeal = new(transform.GetInstanceID());
        }
        private void OnTriggerEnter(Collider other)
        {
            HealableHPComponent.ReceiveHeal(other.transform.root, receiveHeal);
            //EventBus<ReceiveHealOverTime>.Raise(other.transform.root.GetInstanceID(), @receiveHeal);
        }
        private void OnTriggerExit(Collider other)
        {
            HealableHPComponent.RemoveHeal(other.transform.root, removeHeal);
            //EventBus<RemoveEffect>.Raise(other.transform.root.GetInstanceID(), removeHeal);
        }
    }
}