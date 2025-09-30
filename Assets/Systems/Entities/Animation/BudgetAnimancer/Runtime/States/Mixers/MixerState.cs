using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Playables;
namespace BudgetAnimancer
{
    public abstract class MixerState : BudgetAnimancerState
    {
        protected PlayableGraph graph;
        protected List<BudgetAnimancerState> children = new();
        public MixerState(PlayableGraph graph, int index) : base(AnimationMixerPlayable.Create(graph), index)
        {
            this.graph = graph;
        }
        public override void Update() { }
        protected abstract void ParameterValueChanged();
    }
}