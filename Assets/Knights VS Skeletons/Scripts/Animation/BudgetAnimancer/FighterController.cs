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
    protected int locomotionKey = 0;
    [SerializeField] protected UnityEvent dealDamage;
    protected virtual void Start()
    {
        agent = transform.root.GetComponent<NavMeshAgent>();
        component = transform.root.GetComponentInChildren<BudgetAnimancerComponent>();
        attackState = component.CreateOrGetState(attack);
        attackState.OnEnd += ReturnToDefault;
        attackState.AddEvent(0.33f, () => dealDamage?.Invoke());
        locomotionState = component.Layers[0].GetOrAddLinearMixer(locomotionKey, locomotionData);
        ReturnToDefault();
    }
    protected void Update()
    {
        locomotionState.Parameter = agent.velocity.magnitude;
    }
    public void ReturnToDefault()
    {
        component.Layers[0].PlayLinearMixer(locomotionKey);
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
