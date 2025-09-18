using BT;
using UnityEngine;
public class SkeletonBT : MeleeBT
{
    [SerializeField] protected float wanderRadius = 30;
    [SerializeField] protected int nrOfWanderTries = 5;
    protected override void SetupTree()
    {
        base.SetupTree();

        root.AddChild(new LeafNode("Wander", () =>
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3? v = Utilities.Utilities.GetRandomPointOnMesh(wanderRadius, agent, nrOfWanderTries);
                if (v != null)
                {
                    agent.SetDestination(v.Value);
                }
            }
            return NodeState.RUNNING;
        }, () =>
        {
            agent.isStopped = false;
            Vector3? v = Utilities.Utilities.GetRandomPointOnMesh(wanderRadius, agent, nrOfWanderTries);
            if (v != null)
            {
                agent.SetDestination(v.Value);
            }
        }));
    }
}
