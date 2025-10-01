using Animation;
using UnityEngine.AI;
public class SkeletonAnimController : AnimationController
{
    protected NavMeshAgent agent;
    protected override void Awake()
    {
        base.Awake();
        agent = transform.root.GetComponent<NavMeshAgent>();
    }
    protected void Update()
    {
        animator.SetFloat(SkeletonHashes.SpeedFloat, agent.velocity.magnitude);
    }
    public void ReturnToIdle()
    {
        ChangeAnimatorState(SkeletonHashes.BaseLayer_LocomotionState);
    }
    public void Attack()
    {
        ChangeAnimatorState(SkeletonHashes.BaseLayer_AttackState);
    }
    public void Die()
    {
        ChangeAnimatorState(SkeletonHashes.BaseLayer_DeathState);
    }
}
