using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Detection
{
    public class BasicDetector : DetectorBase
    {
        [SerializeField] protected LayerMask targetMask, obstructionMask = 1 << 0;
        protected Collider[] colliders = new Collider[20];
        [SerializeField] protected float radius;
        [field: SerializeField] public Transform ClosestTarget { get; protected set; }
        public UnityEvent<Transform> TargetChanged;
        protected override void HandleDetection()
        {
            if (ClosestTarget == null || !ClosestTarget.gameObject.activeSelf || !CanSee(ClosestTarget.position))
            {
                Transform newTarget = null;
                int nrOfTargets = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, targetMask);
                HashSet<int> set = new() { transform.root.GetInstanceID() };
                for (int i = 0; i < nrOfTargets; i++)
                {
                    var tr = colliders[i].transform.root;
                    if (set.Contains(tr.GetInstanceID())) continue;
                    if (CanSee(tr.position))
                    {
                        set.Add(tr.GetInstanceID());
                        if (newTarget == null)
                        {
                            newTarget = tr;
                            continue;
                        }
                        if (Vector3.Distance(transform.position, newTarget.position) > Vector3.Distance(transform.position, tr.position))
                        {
                            newTarget = tr;
                        }
                    }
                }
                if (newTarget != ClosestTarget)
                {
                    TargetChanged?.Invoke(newTarget);
                    ClosestTarget = newTarget;
                }
            }
        }
        protected virtual bool CanSee(Vector3 pos)
        {
            return !Physics.Linecast(transform.position, pos, obstructionMask);
        }
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
            if (ClosestTarget != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, ClosestTarget.position);
            }
        }
    }
}