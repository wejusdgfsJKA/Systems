using BT;
using UnityEngine;
using UnityEngine.AI;
public class KnightBT : MeleeBT
{
    [SerializeField] protected float wanderRadius = 30;
    [SerializeField] protected int nrOfWanderTries = 5;
    [SerializeField] protected Transform[] patrolPoints;
    protected int currentPatrolPoint;
    protected override void OnEnable()
    {
        base.OnEnable();
        currentPatrolPoint = 0;
    }
    protected override void SetupTree()
    {
        base.SetupTree();

        if (patrolPoints != null && patrolPoints.Length > 1) root.AddChild(GetPatrolNode());
        else root.AddChild(GetWanderNode());
    }
    protected LeafNode GetPatrolNode()
    {
        return new LeafNode("Patrol", () =>
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPatrolPoint].position);
            }
            return NodeState.RUNNING;
        }, () =>
        {
            agent.isStopped = false;
            agent.speed = regularSpeed;
            agent.SetDestination(patrolPoints[currentPatrolPoint].position);
        });
    }
    protected LeafNode GetWanderNode()
    {
        return new LeafNode("Wander", () =>
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
            agent.speed = regularSpeed;
            agent.isStopped = false;
            Vector3? v = NewWanderPoint();
            if (v != null)
            {
                agent.SetDestination(v.Value);
            }
        });
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
    protected virtual void OnDrawGizmos()
    {
        if (patrolPoints != null && patrolPoints.Length > 1)
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < patrolPoints.Length - 1; i++)
            {
                if (patrolPoints[i] != null && patrolPoints[i + 1] != null)
                {
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                }
            }
            if (patrolPoints[0] != null && patrolPoints[patrolPoints.Length - 1] != null)
            {
                Gizmos.DrawLine(patrolPoints[patrolPoints.Length - 1].position, patrolPoints[0].position);
            }
        }
    }
}
