using UnityEngine;
using UnityEngine.Events;

public class OneShotAnimState : CharacterState
{
    public AnimationClip Clip;
    [SerializeField] protected UnityEvent onEnter, onExit;
    public override void Enter()
    {
        base.Enter();
        onEnter?.Invoke();
        Controller.Component.Play(Clip);
    }
    public override void Exit()
    {
        base.Exit();
        onExit?.Invoke();
    }
}