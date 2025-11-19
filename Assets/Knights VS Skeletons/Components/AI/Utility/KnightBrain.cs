using EventBus;
using Tags;
using UnityEngine;
namespace KvS.Utility
{
    public class KnightBrain : FighterBrain
    {
        [SerializeField] protected Transform[] guardArea;
        [SerializeField] protected int healingAreaDistCheck = 30;
        [SerializeField] protected Transform healArea;
        protected override void Awake()
        {
            var list = TagManager<AreaType>.GetByTag(AreaType.HealArea);
            if (list.Count > 0) healArea = list[0];
            base.Awake();
            EventBus<AssignGuardArea>.AddActions(tr.GetInstanceID(), SetGuardArea);
        }
        protected virtual void OnDestroy()
        {
            EventBus<AssignGuardArea>.RemoveBinding(tr.GetInstanceID());
        }
        protected override void SetupContext()
        {
            base.SetupContext();
            Context.SetData<float>(UtilityAI.ContextDataKeys.CurrentHP, 1);
            Context.SetData(UtilityAI.ContextDataKeys.CurrentPatrolPoint, 0);
            Context.SetData(UtilityAI.ContextDataKeys.DistToGuardArea, Mathf.Infinity);
            Context.SetData(UtilityAI.ContextDataKeys.GuardArea, guardArea);
            Context.SetData(UtilityAI.ContextDataKeys.HealAreaDist, Mathf.Infinity);
            Context.SetData(UtilityAI.ContextDataKeys.ClosestHealArea, healArea);
        }
        protected override void UpdateContext()
        {
            base.UpdateContext();
            if (guardArea != null && guardArea.Length > 0)
            {
                int currentPoint = 0;
                if (guardArea.Length > 1)
                {
                    currentPoint = Context.GetData<int>(UtilityAI.ContextDataKeys.CurrentPatrolPoint);
                }
                Context.SetData(UtilityAI.ContextDataKeys.DistToGuardArea,
                    Vector3.Distance(tr.position, guardArea[currentPoint].position));
            }
            if (Context.GetData<float>(UtilityAI.ContextDataKeys.CurrentHP) < 1)
            {
                Context.SetData(UtilityAI.ContextDataKeys.HealAreaDist, Vector3.Distance(healArea.position, transform.position));
            }
        }
        public void SetHPPercentage(float percentage)
        {
            Context.SetData(UtilityAI.ContextDataKeys.CurrentHP, percentage);
        }
        public void SetGuardArea(AssignGuardArea @event)
        {
            guardArea = @event.GuardArea;
            Context.SetData(UtilityAI.ContextDataKeys.GuardArea, guardArea);
            if (guardArea == null || guardArea.Length == 0)
            {
                Context.SetData(UtilityAI.ContextDataKeys.DistToGuardArea, Mathf.Infinity);
            }
        }
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