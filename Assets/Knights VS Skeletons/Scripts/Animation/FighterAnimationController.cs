using Animation;
using UnityEngine.AI;
public class FighterAnimationController : AnimationController
{
    protected NavMeshAgent agent;
    protected override void Awake()
    {
        base.Awake();
        agent = transform.root.GetComponent<NavMeshAgent>();
    }
    protected void Update()
    {
        animator.SetFloat(FighterHashes.SpeedFloat, agent.velocity.magnitude);
    }
    public void ReturnToIdle()
    {
        ChangeAnimatorState(FighterHashes.BaseLayer_LocomotionState);
    }
    public void Attack()
    {
        ChangeAnimatorState(FighterHashes.BaseLayer_AttackState);
    }
    public void Die()
    {
        transform.root.gameObject.SetActive(false);
    }
}
