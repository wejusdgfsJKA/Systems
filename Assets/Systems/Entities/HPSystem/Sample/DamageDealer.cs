using HP;
using UnityEngine;
namespace Sample
{
    public class DamageDealer : MonoBehaviour
    {
        public bool b;
        public Transform target;
        public Collider coll;
        public int damage;
        void Update()
        {
            if (b)
            {
                DealDamage();
                b = false;
            }
        }
        public void DealDamage() => DamageTaker.TakeDamage(target, new TakeDamage(damage, transform, coll));
    }
}