using Animation;
using UnityEngine.AI;
public class AnimController : AnimationController
{
    protected NavMeshAgent agent;
    protected override void Awake()
    {
        base.Awake();
        agent = transform.root.GetComponent<NavMeshAgent>();
    }
    protected void Update()
    {
        animator.SetFloat(AnimatorHashes.SpeedFloat, agent.velocity.magnitude);
    }
    public void ReturnToIdle()
    {
        ChangeState(AnimatorHashes.LocomotionState);
    }
    public void Attack()
    {
        ChangeState(AnimatorHashes.AttackState);
    }
    public void Die()
    {
        transform.root.gameObject.SetActive(false);
    }
}
