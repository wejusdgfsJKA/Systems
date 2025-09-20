using UnityEngine;
using UtilityAI;

[CreateAssetMenu(menuName = "UtilityAI/Actions/Guard")]
public class GuardAction : AIAction
{
    public override void Enter(Context context)
    {
        context.Agent.isStopped = false;
        context.Agent.speed = context.RegularSpeed;
        var guardArea = context.GetData<Transform[]>(ContextDataKeys.GuardArea);
        if (guardArea == null) return;
        if (guardArea.Length == 1)
        {
            context.Agent.SetDestination(guardArea[0].position);
            return;
        }
    }
    public override void Execute(Context context, float deltaTime)
    {
        var guardArea = context.GetData<Transform[]>(ContextDataKeys.GuardArea);
        if (guardArea == null || guardArea.Length == 0) return;
        if (guardArea.Length == 1)
        {
            var tr = context.GetData<Transform>(ContextDataKeys.Target);
            if (tr == null) return;
            if (context.Agent.velocity.magnitude == 0)
            {
                context.Transform.LookAt(new Vector3(tr.position.x, context.Transform.position.y, tr.position.z));
            }
            return;
        }
        int currentPatrolPoint = context.GetData<int>(ContextDataKeys.CurrentPatrolPoint);
        if (context.Agent.remainingDistance <= context.Agent.stoppingDistance)
        {
            currentPatrolPoint = (currentPatrolPoint + 1) % guardArea.Length;
            context.Agent.SetDestination(guardArea[currentPatrolPoint].position);
            context.SetData(ContextDataKeys.CurrentPatrolPoint, currentPatrolPoint);
        }
    }
}
