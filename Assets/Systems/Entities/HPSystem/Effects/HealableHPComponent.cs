using Effects;
using EventBus;
using System.Collections.Generic;
using UnityEngine;
namespace HP
{
    public class HealableHPComponent : HPComponent, IHealable
    {
        protected readonly Dictionary<int, IEffect<IHealable>> activeEffects = new();
        protected override void Awake()
        {
            base.Awake();
            EventBus<ReceiveHealOverTime>.AddActions(transform.GetInstanceID(), ReceiveHeal);
            EventBus<RemoveEffect>.AddActions(transform.GetInstanceID(), RemoveEffect);
        }
        public void Heal(int amount)
        {
            CurrentHealth += amount;
        }
        /// <summary>
        /// Attempt to apply a heal over time effect to a transform.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="heal"></param>
        /// <returns>True if the effect could be added.</returns>
        public static bool ReceiveHeal(Transform transform, ReceiveHealOverTime heal)
        {
            return EventBus<ReceiveHealOverTime>.Raise(transform.GetInstanceID(), heal);
        }
        public static bool RemoveEffect(Transform transform, RemoveEffect @event)
        {
            return EventBus<RemoveEffect>.Raise(transform.GetInstanceID(), @event);
        }
        public void ReceiveHeal(ReceiveHealOverTime heal)
        {
            ApplyEffect(heal.Effect);
        }
        public void ApplyEffect(IEffect<IHealable> effect)
        {
            if (!gameObject.activeSelf) return;
            effect.OnCompleted += RemoveEffect;
            activeEffects.TryAdd(effect.ID, effect);
            effect.Apply(this);
            OnDeath.AddListener(RemoveAllEffects);
        }
        public void RemoveAllEffects()
        {
            foreach (var effect in activeEffects.Values)
            {
                effect.OnCompleted -= RemoveEffect;
                effect.Cancel();
            }
            activeEffects.Clear();
        }
        protected void RemoveEffect(RemoveEffect @event) => RemoveEffect(@event.EffectID);
        protected void RemoveEffect(IEffect<IHealable> effect) => RemoveEffect(effect.ID);
        protected void RemoveEffect(int effectID)
        {
            if (activeEffects.TryGetValue(effectID, out var effect))
            {
                effect.OnCompleted -= RemoveEffect;
                effect.Cancel();
                activeEffects.Remove(effectID);
            }
        }
    }
}