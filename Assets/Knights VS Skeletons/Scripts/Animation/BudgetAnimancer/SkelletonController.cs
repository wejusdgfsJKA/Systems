using BudgetAnimancer;
using FSM;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class SkelletonController : MonoBehaviour
{
    public BudgetAnimancerComponent Component { get; private set; }
    StateMachine stateMachine;
    public NavMeshAgent Agent { get; private set; }
    [SerializeField] CharacterState locomotionState;
    [SerializeField] OneShotAnimState attackState, deathState;
    [SerializeField] protected UnityEvent dealDamage;
    [SerializeField] protected UnityEvent die;
    private void Awake()
    {
        Component = GetComponent<BudgetAnimancerComponent>();
        Agent = transform.root.GetComponent<NavMeshAgent>();
        locomotionState.Controller = this;
        attackState.Controller = this;
        deathState.Controller = this;
        stateMachine = new(locomotionState);
        var state1 = Component.CreateOrGetState(attackState.Clip);
        state1.AddEvent(0.33f, () => dealDamage?.Invoke());
        state1.OnEnd += ReturnToIdle;
        var state2 = Component.CreateOrGetState(deathState.Clip);
        state2.OnEnd += () => die?.Invoke();
    }
    private void OnEnable()
    {
        ReturnToIdle();
    }
    public void ReturnToIdle()
    {
        stateMachine.ReturnToDefault(true);
    }
    public void Attack()
    {
        stateMachine.ChangeState(attackState);
    }
    public void Die()
    {
        stateMachine.ForceSetState(deathState);
    }
}
