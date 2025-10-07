using UnityEngine.Animations;
using UnityEngine.Playables;
namespace BudgetAnimancer
{
    /// <summary>
    /// Wrapper for an AnimationMixerPlayable. Would make this generic but C# doesn't know if T supports 
    /// equality, oh well.
    /// </summary>
    public abstract class MixerState : BudgetAnimancerState
    {
        protected PlayableGraph graph;
        public MixerState(PlayableGraph graph, int index) : base(AnimationMixerPlayable.Create(graph), index)
        {
            this.graph = graph;
        }
        public override void Update() { }
        /// <summary>
        /// Meant to be called when the value of the parameter changes to recalculate weights.
        /// </summary>
        protected abstract void ParameterValueChanged();
    }
}