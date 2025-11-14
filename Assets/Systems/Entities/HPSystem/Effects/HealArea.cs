using HP;
using Tags;
using UnityEngine;
namespace Effects
{
    public class HealArea : Taggable<AreaType>
    {
        [SerializeField] int healAmount = 1;
        RemoveEffect removeHeal;
        private void Awake()
        {
            currentTags ??= new() { AreaType.HealArea };
            removeHeal = new(transform.GetInstanceID());
        }
        private void OnTriggerEnter(Collider other)
        {
            HealableHPComponent.ReceiveHeal(other.transform.root, new(transform.GetInstanceID(),
                float.PositiveInfinity, 1, healAmount));
            //EventBus<ReceiveHealOverTime>.Raise(other.transform.root.GetInstanceID(), @receiveHeal);
        }
        private void OnTriggerExit(Collider other)
        {
            HealableHPComponent.RemoveEffect(other.transform.root, removeHeal);
            //EventBus<RemoveEffect>.Raise(other.transform.root.GetInstanceID(), removeHeal);
        }
    }
}