using HybridBT;
using System;
using UnityEngine;
namespace KvS.Hybrid
{
    [CreateAssetMenu(menuName = "HybridBT/KvS/ChaseNode")]
    public class ChaseNodeData : LeafNodeData<KvS_Keys>
    {
        [SerializeField] protected float chaseSpeed = 5;
        [SerializeField] protected float chaseErrorThreshold = 0.1f;
        protected override Action<Context<KvS_Keys>> onEnter => (context) =>
        {
            var agent = context.Agent;
            var targetPos = context.GetData<Transform>(KvS_Keys.Target).position;
            context.SetData(KvS_Keys.PrevTargetPos, targetPos);
            agent.destination = targetPos;
            agent.isStopped = false;
            agent.speed = chaseSpeed;
        };
        protected override Func<Context<KvS_Keys>, NodeState> onEvaluate => (context) =>
        {
            var prevPos = context.GetData<Vector3>(KvS_Keys.PrevTargetPos);
            var targetPos = context.GetData<Transform>(KvS_Keys.Target).position;
            if ((prevPos - targetPos).magnitude > chaseErrorThreshold)
            {
                context.Agent.destination = targetPos;
                context.SetData(KvS_Keys.PrevTargetPos, targetPos);
            }
            return NodeState.RUNNING;
        };
    }
}