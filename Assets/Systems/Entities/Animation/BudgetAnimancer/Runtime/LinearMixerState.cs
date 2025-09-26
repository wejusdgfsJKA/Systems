using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
namespace BudgetAnimancer
{
    public class MixerState : BudgetAnimancerState
    {
        public readonly AnimationMixerPlayable Mixer;
        protected PlayableGraph graph;
        protected List<BudgetAnimancerState> children = new();
        public MixerState(PlayableGraph graph, AnimationMixerPlayable mixer, int index) : base(mixer, index)
        {
            this.graph = graph;
            Mixer = mixer;
        }

        protected void AddInput(BudgetAnimancerState state)
        {
            var count = Mixer.GetInputCount();
            Mixer.SetInputCount(count + 1);
            Mixer.SetInputWeight(count + 1, 0);
            graph.Connect(state.Playable, 0, Mixer, count);
            children.Add(state);
        }
        public override void Update()
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].Update();
            }
        }
    }
    public class LinearMixerState : MixerState
    {
        protected float parameter;
        protected List<float> thresholds = new();
        public float Parameter
        {
            get { return parameter; }
            set
            {
                if (parameter != value)
                {
                    parameter = value;
                    ParameterValueChanged();
                }
            }
        }
        (int, int) prevThresholds = new(0, 0);
        public LinearMixerState(PlayableGraph graph, AnimationMixerPlayable mixer, int index) :
            base(graph, mixer, index)
        { }

        protected void ParameterValueChanged()
        {
            switch (thresholds.Count)
            {
                case 0:
                    {
                        Debug.LogError("No thresholds set!");
                        return;
                    }
                case 1:
                    {
                        Mixer.SetInputWeight(0, 1);
                        return;
                    }
                default:
                    {
                        if (!(thresholds[prevThresholds.Item1] < parameter && parameter < thresholds[prevThresholds.Item2]))
                        {
                            if (thresholds[0] == parameter)
                            {
                                prevThresholds.Item1 = prevThresholds.Item2 = 0;
                            }
                            else
                            {
                                for (int i = 0; i < thresholds.Count - 1; i++)
                                {
                                    if (thresholds[i + 1] == parameter)
                                    {
                                        prevThresholds.Item1 = prevThresholds.Item2 = i + 1;
                                        break;
                                    }
                                    if (thresholds[i] < parameter && parameter < thresholds[i + 1])
                                    {
                                        prevThresholds.Item1 = i;
                                        prevThresholds.Item2 = i + 1;
                                        break;
                                    }
                                }
                            }
                        }
                        //actually do the blending (no clue how, random bullshit go!)
                        PerformBlend();
                        break;
                    }
            }
        }
        public void PerformBlend()
        {
            if (prevThresholds.Item1 == prevThresholds.Item2)
            {
                Mixer.SetInputWeight(prevThresholds.Item1, 1);
                return;
            }
            float normalized = (parameter - thresholds[prevThresholds.Item1]) / (thresholds[prevThresholds.Item2] - thresholds[prevThresholds.Item1]);
            Mixer.SetInputWeight(prevThresholds.Item1, 1 - normalized);
            Mixer.SetInputWeight(prevThresholds.Item2, normalized);
        }
        public void AddMotion(AnimationClip clip, float threshold)
        {
            var playable = AnimationClipPlayable.Create(graph, clip);
            var index = thresholds.Count;

            Mixer.SetInputCount(index + 1);
            Mixer.SetInputWeight(index + 1, 0);
            graph.Connect(playable, 0, Mixer, index);
            thresholds.Add(threshold);
            ParameterValueChanged();
        }
    }
}