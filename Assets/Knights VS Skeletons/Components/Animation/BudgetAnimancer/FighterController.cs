using BudgetAnimancer;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class FighterController : MonoBehaviour
{
    protected BudgetAnimancerComponent component;
    protected NavMeshAgent agent;
    [SerializeField] protected LinearMixerStateData locomotionData;
    [SerializeField] protected AnimationClip attack;
    protected LinearMixerState locomotionState;
    protected AnimState attackState;
    [SerializeField] protected UnityEvent dealDamage;
    protected virtual void Awake()
    {
        agent = transform.root.GetComponent<NavMeshAgent>();
        component = transform.root.GetComponentInChildren<BudgetAnimancerComponent>();
        attackState = component.CreateOrGetAnimationState(attack);
        attackState.OnEnd += ReturnToIdle;
        attackState.AddEvent(0.33f, () => dealDamage?.Invoke());
        locomotionState = component.Layers[0].GetOrAddLinearMixer(locomotionData);
    }
    private void OnEnable()
    {
        ReturnToIdle();
    }
    protected void Update()
    {
        locomotionState.Parameter = agent.velocity.magnitude;
    }
    public void ReturnToIdle()
    {
        component.Layers[0].PlayLinearMixer(locomotionData.Key);
    }
    public void Attack()
    {
        component.Play(attack);
    }
    public virtual void Die()
    {
        transform.root.gameObject.SetActive(false);
    }
}
