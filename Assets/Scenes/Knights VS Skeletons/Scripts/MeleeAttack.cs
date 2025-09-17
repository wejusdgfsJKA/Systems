using HP;
using System.Collections.Generic;
using UnityEngine;
namespace Weapons.Melee
{
    public class MeleeAttack : MonoBehaviour
    {
        [SerializeField] int damage;
        [SerializeField] LayerMask targetMask = 1 << 6;
        [SerializeField] MeleeHitbox[] hitboxes;
        static Collider[] colliders;
        TakeDamage dmg;
        private void Awake()
        {
            dmg = new(damage, transform.root);
        }
        public void DealDamage()
        {
            colliders = new Collider[GlobalConfig.MaxTargets];
            HashSet<int> set = new() { transform.root.GetInstanceID() };
            for (int i = 0; i < hitboxes.Length; i++)
            {
                var h = hitboxes[i];
                Vector3 pos = transform.position + transform.forward * h.Offset.z +
                    transform.right * h.Offset.x + transform.up * h.Offset.y;
                int nrOfTargets = Physics.OverlapSphereNonAlloc(pos, h.Radius,
                    colliders, targetMask);
                for (int j = 0; j < nrOfTargets; j++)
                {
                    var tr = colliders[j].transform.root;
                    if (set.Contains(tr.GetInstanceID())) continue;
                    set.Add(tr.GetInstanceID());
                    HPComponent.TakeDamage(tr, dmg);
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < hitboxes.Length; i++)
            {
                var h = hitboxes[i];
                Vector3 pos = transform.position +
                    transform.forward * h.Offset.z +
                    transform.right * h.Offset.x +
                    transform.up * h.Offset.y;
                Gizmos.DrawWireSphere(pos, h.Radius);
            }
        }
    }
}
