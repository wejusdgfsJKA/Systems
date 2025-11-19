using HybridBT;
using System;
using UnityEngine;
namespace KvS.Hybrid
{
    [CreateAssetMenu(menuName = "HybridBT/KvS/GuardNode")]
    public class GuardNodeData : LeafNodeData<KvS_Keys>
    {
        [SerializeField] protected float regularSpeed = 3.5f;
        protected override Action<Context<KvS_Keys>> onEnter => (context) =>
        {
            var agent = context.Agent;
            agent.isStopped = false;
            agent.speed = regularSpeed;
            var guardArea = context.GetData<Transform[]>(KvS_Keys.GuardArea);
            if (guardArea == null) return;
            int currentPatrolPoint = 0;
            if (guardArea.Length > 1)
            {
                currentPatrolPoint = context.GetData<int>(KvS_Keys.CurrentPatrolPoint);
            }
            agent.SetDestination(guardArea[currentPatrolPoint].position);
        };
        protected override Func<Context<KvS_Keys>, NodeState> onEvaluate => (context) =>
        {
            var guardArea = context.GetData<Transform[]>(KvS_Keys.GuardArea);
            if (guardArea == null || guardArea.Length == 0) return NodeState.FAILURE;
            var agent = context.Agent;
            if (guardArea.Length == 1)
            {
                if (agent.velocity.magnitude == 0)
                {
                    var tr = context.GetData<Transform>(KvS_Keys.Target);
                    if (tr != null) context.Transform.LookAt(new Vector3(tr.position.x,
                        context.Transform.position.y, tr.position.z));
                }
                return NodeState.RUNNING;
            }
            int currentPatrolPoint = context.GetData<int>(KvS_Keys.CurrentPatrolPoint);
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                currentPatrolPoint = (currentPatrolPoint + 1) % guardArea.Length;
                agent.SetDestination(guardArea[currentPatrolPoint].position);
                context.SetData(KvS_Keys.CurrentPatrolPoint, currentPatrolPoint);
            }
            return NodeState.RUNNING;
        };
    }
}