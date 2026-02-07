using UnityEngine;
using UnityEngine.Events;

namespace HP
{
    public class HPComponent : DamageTaker
    {
        [field: SerializeField] public int MaxHealth { get; set; }
        [SerializeField] protected float currentHealth;
        /// <summary>
        /// Current health of the object. Invokes OnHealthChanged and its string counterpart when the value changes.
        /// </summary>
        public float CurrentHealth
        {
            get
            {
                return currentHealth;
            }
            set
            {
                value = Mathf.Clamp(value, 0, MaxHealth);
                if (value != currentHealth)
                {
                    currentHealth = value;
                    OnHealthChanged?.Invoke(currentHealth / MaxHealth);
                    OnHealthChangedString?.Invoke((currentHealth / MaxHealth).ToString());
                }
            }
        }
        /// <summary>
        /// Fires when this entity's health value changes. Has as parameter the 
        /// entity's current health percentage in float format.
        /// </summary>
        public UnityEvent<float> OnHealthChanged;
        public UnityEvent<string> OnHealthChangedString;
        /// <summary>
        /// Fires when this entity dies.
        /// </summary>
        public UnityEvent OnDeath;
        protected void OnEnable()
        {
            CurrentHealth = MaxHealth;
        }

        /// <summary>
        /// Take damage. Invoke OnDeath when HP reaches 0. 
        /// </summary>
        /// <param name="dmg">Damage Event.</param>
        public override void TakeDamage(TakeDamage dmg)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - CalculateDamage(dmg), 0, MaxHealth);
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
    }
}