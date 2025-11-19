using EventBus;
using HybridBT;
using Tags;
using UnityEngine;
using UnityEngine.Events;
namespace KvS.Hybrid
{
    public class KnightHybridBT : BT<KvS_Keys>
    {
        [field: SerializeField] public Transform Target { get; protected set; }
        protected Transform tr;
        [SerializeField] protected Transform[] guardArea;
        [SerializeField] protected int healingAreaDistCheck = 30;
        [SerializeField] protected Transform healArea;
        [SerializeField] protected UnityEvent attackEvent;
        protected override void Awake()
        {
            tr = transform;
            var list = TagManager<AreaType>.GetByTag(AreaType.HealArea);
            if (list.Count > 0) healArea = list[0];
            EventBus<AssignGuardArea>.AddActions(tr.GetInstanceID(), SetGuardArea);
            EventBus<AttackEvent>.AddActions(tr.GetInstanceID(), null, Attack);
            base.Awake();
        }
        protected virtual void OnDestroy()
        {
            EventBus<AssignGuardArea>.RemoveBinding(tr.GetInstanceID());
        }
        protected override void SetupBlackboard()
        {
            blackboard.SetData<Transform>(KvS_Keys.Target, null);
            blackboard.SetData(KvS_Keys.DistToTarget, Mathf.Infinity);
            blackboard.SetData<float>(KvS_Keys.CurrentHPPercentage, 1);
            blackboard.SetData(KvS_Keys.CurrentPatrolPoint, 0);
            blackboard.SetData(KvS_Keys.DistToGuardArea, Mathf.Infinity);
            blackboard.SetData(KvS_Keys.GuardArea, guardArea);
            blackboard.SetData(KvS_Keys.HealAreaDist, Mathf.Infinity);
            blackboard.SetData(KvS_Keys.ClosestHealArea, healArea);
        }
        #region Helpers
        public void Attack() => attackEvent?.Invoke();
        public void SetGuardArea(AssignGuardArea @event)
        {
            guardArea = @event.GuardArea;
            blackboard.SetData(KvS_Keys.GuardArea, guardArea);
            if (guardArea == null || guardArea.Length == 0)
            {
                blackboard.SetData(KvS_Keys.DistToGuardArea, Mathf.Infinity);
            }
        }
        public void SetHPPercentage(float percentage)
        {
            blackboard.SetData(KvS_Keys.CurrentHPPercentage, percentage);
        }
        public void SetTarget(Transform target)
        {
            Target = target;
            blackboard.SetData(KvS_Keys.Target, target);
            if (target == null) blackboard.SetData(KvS_Keys.DistToTarget, Mathf.Infinity);
        }
        #endregion
        protected void OnDrawGizmosSelected()
        {
            if (guardArea != null)
            {
                Gizmos.color = Color.magenta;
                if (guardArea.Length < 1)
                {
                    return;
                }
                Vector3 center = guardArea[0].position;
                for (int i = 0; i < guardArea.Length - 1; i++)
                {
                    center += guardArea[i + 1].position;
                    if (guardArea[i] != null && guardArea[i + 1] != null)
                    {
                        Gizmos.DrawLine(guardArea[i].position, guardArea[i + 1].position);
                    }
                }
                center /= guardArea.Length;
                Gizmos.DrawWireSphere(center, 1);
                if (guardArea[0] != null && guardArea[^1] != null)
                {
                    Gizmos.DrawLine(guardArea[^1].position, guardArea[0].position);
                }
            }
        }
    }
}