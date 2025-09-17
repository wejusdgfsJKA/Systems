using BT;
using UnityEngine;
using UnityEngine.AI;
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
                Vector3? v = NewWanderPoint();
                if (v != null)
                {
                    agent.SetDestination(v.Value);
                }
            }
            return NodeState.RUNNING;
        }, () =>
        {
            agent.isStopped = false;
            Vector3? v = NewWanderPoint();
            if (v != null)
            {
                agent.SetDestination(v.Value);
            }
        }));
    }
    protected Vector3? NewWanderPoint()
    {
        NavMeshHit hit;
        Vector3 v = transform.position + Random.onUnitSphere * wanderRadius;
        int c = 0;
        while (!NavMesh.SamplePosition(v, out hit, wanderRadius, agent.areaMask))
        {
            v = transform.position + Random.onUnitSphere * wanderRadius;
            c++;
            if (c > nrOfWanderTries) return null;
        }
        return hit.position;
    }
}
