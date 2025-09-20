using BT;
using UnityEngine;
using UtilityAI;
[CreateAssetMenu(menuName = "UtilityAI/Actions/Offense")]
public class OffenseAction : AIAction
{
    protected Composite root;
    [SerializeField] protected float chaseErrorThreshold = 0.1f;
    [SerializeField] protected float meleeRange = 2;
    public override void Initialize(Context context)
    {
        root = new Selector();
        var agent = context.Agent;
        var attack = new LeafNode("Attack", () =>
        {
            var targetPos = context.GetData<Transform>(ContextDataKeys.Target).position;
            context.Transform.LookAt(new Vector3(targetPos.x, context.Transform.position.y, targetPos.z));
            context.InvokeEvent(ContextEventKeys.Attack);
            return NodeState.RUNNING;
        }, () =>
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        });
        attack.AddDecorator(new Decorator("IsInRange", () =>
        {
            return context.GetData<float>(ContextDataKeys.DistToTarget) <= meleeRange;
        })).MonitorValue(context, ContextDataKeys.DistToTarget);

        root.AddChild(attack);

        //chase node
        root.AddChild(new LeafNode("Chase", () =>
        {
            var prevPos = context.GetData<Vector3>(ContextDataKeys.PrevTargetPos);
            var targetPos = context.GetData<Transform>(ContextDataKeys.Target).position;
            if ((prevPos - targetPos).magnitude > chaseErrorThreshold)
            {
                agent.destination = targetPos;
                context.SetData(ContextDataKeys.PrevTargetPos, targetPos);
            }
            return NodeState.RUNNING;
        }, () =>
        {
            var targetPos = context.GetData<Transform>(ContextDataKeys.Target).position;
            agent.destination = targetPos;
            context.SetData(ContextDataKeys.PrevTargetPos, targetPos);
            agent.isStopped = false;
            agent.speed = context.ChaseSpeed;
        }));
    }

    public override void Execute(Context context, float deltaTime)
    {
        root.Evaluate(deltaTime);
    }

    public override void Exit(Context context)
    {
        root.Abort();
    }
}
