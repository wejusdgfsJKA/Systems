using EventBus;
using UnityEngine;

namespace HP
{
    public abstract class DamageTaker : MonoBehaviour
    {
        protected virtual void Awake()
        {
            //add damage binding for this entity
            EventBus<TakeDamage>.AddActions(transform.GetInstanceID(), TakeDamage);
        }
        /// <summary>
        /// Attempt to deal damage to an object.
        /// </summary>
        /// <param name="transform">The object whose Root that we are trying to damage.</param>
        /// <param name="dmg">The damage Event.</param>
        /// <returns>True if successfully dealt damage to the object.</returns>
        public static bool TakeDamage(Transform transform, TakeDamage dmg)
        {
            return EventBus<TakeDamage>.Raise(transform.GetInstanceID(), dmg);
        }
        public abstract void TakeDamage(TakeDamage dmg);
        protected virtual void OnDestroy()
        {
            //clear this binding from the Event bus.
            EventBus<TakeDamage>.RemoveActions(transform.GetInstanceID(), TakeDamage);
        }
    }
}