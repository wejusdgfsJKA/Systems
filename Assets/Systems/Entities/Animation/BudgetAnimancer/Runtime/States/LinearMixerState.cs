using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
namespace BudgetAnimancer
{
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
        public LinearMixerState(PlayableGraph graph, int index, List<(float, AnimationClip, float)> motionFields,
            float initialParameterValue = 0) :
            base(graph, index)
        {
            var Mixer = (AnimationMixerPlayable)Playable;
            Mixer.SetInputCount(motionFields.Count);
            for (int i = 0; i < motionFields.Count; i++)
            {
                var playable = AnimationClipPlayable.Create(graph, motionFields[i].Item2);
                playable.SetSpeed(motionFields[i].Item3);
                Mixer.SetInputWeight(i, 0);
                graph.Connect(playable, 0, Mixer, i);
                thresholds.Add(motionFields[i].Item1);
            }
            parameter = initialParameterValue;
            ParameterValueChanged();
        }

        protected override void ParameterValueChanged()
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
                        ((AnimationMixerPlayable)Playable).SetInputWeight(0, 1);
                        return;
                    }
                default:
                    {
                        if (!(thresholds[prevThresholds.Item1] < parameter && parameter <
                            thresholds[prevThresholds.Item2]))
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
            var Mixer = (AnimationMixerPlayable)Playable;
            if (prevThresholds.Item1 == prevThresholds.Item2)
            {
                Mixer.SetInputWeight(prevThresholds.Item1, 1);
                return;
            }
            float normalized = Mathf.Clamp01((parameter - thresholds[prevThresholds.Item1]) /
                (thresholds[prevThresholds.Item2] - thresholds[prevThresholds.Item1]));
            Mixer.SetInputWeight(prevThresholds.Item1, 1 - normalized);
            Mixer.SetInputWeight(prevThresholds.Item2, normalized);
        }

        public void AddMotion(AnimationClip clip, float threshold)
        {
            var Mixer = (AnimationMixerPlayable)Playable;
            var playable = AnimationClipPlayable.Create(graph, clip);
            var index = thresholds.Count;

            Mixer.SetInputCount(index + 1);
            Mixer.SetInputWeight(index, 0);
            graph.Connect(playable, 0, Mixer, index);
            thresholds.Add(threshold);
            ParameterValueChanged();
        }
    }
}