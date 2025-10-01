using BudgetAnimancer;
using UnityEngine;

public class ContMixerState : CharacterState
{
    [SerializeField] LinearMixerStateData data;
    public override void Enter()
    {
        base.Enter();
        Controller.Component.Layers[0].PlayLinearMixer(data);
    }
}