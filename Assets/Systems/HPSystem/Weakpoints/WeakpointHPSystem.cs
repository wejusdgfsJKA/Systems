using UnityEngine;
namespace HP.Weakpoints
{
    public class WeakpointHPSystem : HPComponent
    {
        [SerializeField] protected Weakpoint[] weakpoints;
        protected override float CalculateDamage(TakeDamage dmg)
        {
            for (int i = 0; i < weakpoints.Length; i++)
            {
                if (weakpoints[i].Collider.GetInstanceID() == dmg.ColliderID)
                {
                    return base.CalculateDamage(dmg) * weakpoints[i].Modifier;
                }
            }
            return base.CalculateDamage(dmg);
        }
    }
}