using EventBus;
using HybridBT;
using System;
using UnityEngine;
namespace KvS.Hybrid
{
    [CreateAssetMenu(menuName = "HybridBT/KvS/MeleeNode")]
    public class MeleeNodeData : LeafNodeData<KvS_Keys>
    {
        protected override Action<Context<KvS_Keys>> onEnter => (context) =>
        {
            var agent = context.Agent;
            if (agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
        };
        protected override Func<Context<KvS_Keys>, NodeState> onEvaluate => (context) =>
            {
                var targetPos = context.GetData<Transform>(KvS_Keys.Target).position;
                context.Transform.LookAt(new Vector3(targetPos.x, context.Transform.position.y, targetPos.z));
                EventBus<AttackEvent>.Raise(context.Transform.GetInstanceID(), null);
                return NodeState.RUNNING;
            };
        protected override Node<KvS_Keys> GetNode(Context<KvS_Keys> context)
        {
            var agent = context.Agent;
            return new LeafNode<KvS_Keys>(Name, onEvaluate, onEnter, () => agent.isStopped = false);
        }
    }
}