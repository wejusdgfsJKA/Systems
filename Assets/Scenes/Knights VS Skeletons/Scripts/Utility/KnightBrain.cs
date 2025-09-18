using UnityEngine;

public class KnightBrain : FighterBrain
{
    [SerializeField] protected Transform[] guardArea;
    protected Vector3? guardAreaCenter = null;
    protected override void SetupContext()
    {
        base.SetupContext();
        if (guardArea != null && guardArea.Length > 0)
        {
            guardAreaCenter = guardArea[0].position;
            for (int i = 1; i < guardArea.Length; i++)
            {
                guardAreaCenter += guardArea[i].position;
            }
            guardAreaCenter /= guardArea.Length;
            Context.SetData(UtilityAI.ContextDataKeys.CurrentPatrolPoint, 0);
        }
        Context.SetData(UtilityAI.ContextDataKeys.DistToGuardArea, Mathf.Infinity);
        Context.SetData(UtilityAI.ContextDataKeys.GuardArea, guardArea);
    }
    protected override void UpdateContext()
    {
        base.UpdateContext();
        if (guardAreaCenter != null)
        {
            Context.SetData(UtilityAI.ContextDataKeys.DistToGuardArea,
                Vector3.Distance(transform.position, guardAreaCenter.Value));
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
            if (guardArea[0] != null && guardArea[guardArea.Length - 1] != null)
            {
                Gizmos.DrawLine(guardArea[guardArea.Length - 1].position, guardArea[0].position);
            }
        }
    }
}
