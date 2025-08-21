using EventBus;
using UnityEngine;
using UnityEngine.Events;

namespace HP
{
    public class HPComponent : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        /// <summary>
        /// Fires when this entity dies.
        /// </summary>
        public UnityEvent OnDeath;
        protected virtual void Awake()
        {
            //add damage binding for this entity
            EventBus<TakeDamage>.AddActions(transform.GetInstanceID(), TakeDamage);
        }
        protected void OnEnable()
        {
            CurrentHealth = MaxHealth;
        }
        /// <summary>
        /// Take damage. Invoke OnDeath when HP reaches 0. 
        /// </summary>
        /// <param name="dmg">Damage event.</param>
        public void TakeDamage(TakeDamage dmg)
        {
            CurrentHealth -= CalculateDamage(dmg);
            if (CurrentHealth <= 0)
            {
                OnDeath?.Invoke();
                return;
            }
        }
        protected virtual float CalculateDamage(TakeDamage dmg)
        {
            return dmg.Damage;
        }
        protected void OnDestroy()
        {
            //clear this binding from the event bus.
            EventBus<TakeDamage>.RemoveActions(transform.GetInstanceID(), TakeDamage);
        }
    }
}