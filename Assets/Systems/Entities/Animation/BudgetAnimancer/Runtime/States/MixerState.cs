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

        protected void AddInput(BudgetAnimancerState state)
        {
            var Mixer = (AnimationMixerPlayable)Playable;
            var count = Mixer.GetInputCount();
            Mixer.SetInputCount(count + 1);
            Mixer.SetInputWeight(count + 1, 0);
            graph.Connect(state.Playable, 0, Mixer, count);
            children.Add(state);
        }
        public override void Update() { }
        protected abstract void ParameterValueChanged();
    }
}