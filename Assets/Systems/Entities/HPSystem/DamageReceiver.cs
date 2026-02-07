using UnityEngine;

namespace HP
{
    public class DamageReceiver : DamageTaker
    {
        protected HPComponent hPComponent;
        protected override void Awake()
        {
            base.Awake();
            hPComponent = GetComponentInParent<HPComponent>();
            if (hPComponent == null)
            {
                Debug.LogError($"DamageReceiver {transform} unable to find HPComponent!");
            }
        }
        public override void TakeDamage(TakeDamage dmg)
        {
            hPComponent.TakeDamage(dmg);
        }
    }
}