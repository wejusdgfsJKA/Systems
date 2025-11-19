using HybridBT;
using System;
using UnityEngine;
using UnityEngine.AI;
namespace KvS.Hybrid
{
    [CreateAssetMenu(menuName = "HybridBT/KvS/MoveToAreaNode")]
    public class MoveToAreaNodeData : LeafNodeData<KvS_Keys>
    {
        [SerializeField] protected KvS_Keys transformKey;
        [SerializeField] protected float speed = 5;
        protected override Action<Context<KvS_Keys>> onEnter => (context) =>
        {
            var agent = context.Agent;
            agent.isStopped = false;
            agent.speed = speed;
            agent.SetDestination(context.GetData<Transform>(transformKey).position);
        };
        protected override Func<Context<KvS_Keys>, NodeState> onEvaluate => (context) =>
        {
            if (context.Agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                context.Agent.SetDestination(context.GetData<Transform>(transformKey).position);
            }
            else if (context.Agent.remainingDistance <= context.Agent.stoppingDistance)
            {
                return NodeState.SUCCESS;
            }
            return NodeState.RUNNING;
        };
    }
}