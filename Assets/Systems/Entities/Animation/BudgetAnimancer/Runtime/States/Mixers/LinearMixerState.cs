using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace BudgetAnimancer
{
    public class LinearMixerState : MixerState
    {
        protected float parameter;
        protected readonly List<float> thresholds = new();

        public float Parameter
        {
            get => parameter;
            set
            {
                if (parameter != value)
                {
                    parameter = value;
                    ParameterValueChanged();
                }
            }
        }

        private (int, int) prevThresholds = new(0, 0);

        public LinearMixerState(PlayableGraph graph, int index, List<(float threshold, AnimationClip clip, float speed)> motionFields,
            float initialParameterValue = 0)
            : base(graph, index)
        {
            Playable.SetInputCount(motionFields.Count);
            var mixer = (AnimationMixerPlayable)Playable;
            motionFields.Sort((a, b) => a.threshold.CompareTo(b.threshold));

            for (int i = 0; i < motionFields.Count; i++)
            {
                var (threshold, clip, speed) = motionFields[i];
                var playable = AnimationClipPlayable.Create(graph, clip);
                playable.SetSpeed(speed);

                // Prevent IK/Root scaling artifacts
                playable.SetApplyFootIK(false);
                playable.SetApplyPlayableIK(false);

                graph.Connect(playable, 0, mixer, i);
                mixer.SetInputWeight(i, 0);

                thresholds.Add(threshold);
            }

            parameter = initialParameterValue;
            ParameterValueChanged();
        }

        public LinearMixerState(PlayableGraph graph, int index, List<MotionField<float>> motionFields,
            float initialParameterValue = 0)
            : base(graph, index)
        {
            motionFields.Sort((a, b) => a.Threshold.CompareTo(b.Threshold));
            Playable.SetInputCount(motionFields.Count);
            var mixer = (AnimationMixerPlayable)Playable;

            for (int i = 0; i < motionFields.Count; i++)
            {
                var (threshold, clip, speed) = (motionFields[i].Threshold, motionFields[i].Clip, motionFields[i].Speed);
                var playable = AnimationClipPlayable.Create(graph, clip);
                playable.SetSpeed(speed);

                // Prevent IK/Root scaling artifacts
                playable.SetApplyFootIK(false);
                playable.SetApplyPlayableIK(false);

                graph.Connect(playable, 0, mixer, i);
                Playable.SetInputWeight(i, 0);

                thresholds.Add(threshold);
            }

            parameter = initialParameterValue;
            ParameterValueChanged();
        }

        public LinearMixerState(PlayableGraph graph, int index, LinearMixerStateData data) : base(graph, index)
        {
            var motionFields = data.MotionFields;
            motionFields.Sort((a, b) => a.Threshold.CompareTo(b.Threshold));
            Playable.SetInputCount(motionFields.Count);
            var mixer = (AnimationMixerPlayable)Playable;

            for (int i = 0; i < motionFields.Count; i++)
            {
                var (threshold, clip, speed) = (motionFields[i].Threshold, motionFields[i].Clip, motionFields[i].Speed);
                var playable = AnimationClipPlayable.Create(graph, clip);
                playable.SetSpeed(speed);

                // Prevent IK/Root scaling artifacts
                playable.SetApplyFootIK(false);
                playable.SetApplyPlayableIK(false);

                graph.Connect(playable, 0, mixer, i);
                Playable.SetInputWeight(i, 0);

                thresholds.Add(threshold);
            }

            parameter = data.DefaultParamValue;
            ParameterValueChanged();
        }

        protected override void ParameterValueChanged()
        {
            if (thresholds.Count == 0)
            {
                Debug.LogError("No thresholds set!");
                return;
            }

            if (thresholds.Count == 1)
            {
                Playable.SetInputWeight(0, 1);
                return;
            }

            if (!(prevThresholds.Item1 < parameter && parameter < prevThresholds.Item2) &&
                !(prevThresholds.Item1 == parameter && parameter == prevThresholds.Item2))
            {
                if (parameter <= thresholds[0]) prevThresholds = (0, 0);
                else if (parameter >= thresholds[thresholds.Count - 1]) prevThresholds = (thresholds.Count - 1, thresholds.Count - 1);
                else
                {
                    for (int i = 0; i < thresholds.Count - 1; i++)
                    {
                        if (parameter == thresholds[i])
                        {
                            prevThresholds = (i, i);
                            break;
                        }
                        if (thresholds[i] < parameter && parameter < thresholds[i + 1])
                        {
                            prevThresholds = (i, i + 1);
                            break;
                        }
                    }
                }
            }
            PerformBlend();
        }

        private void PerformBlend()
        {
            var mixer = (AnimationMixerPlayable)Playable;

            // Clear all weights first
            for (int i = 0; i < thresholds.Count; i++)
                mixer.SetInputWeight(i, 0);

            if (prevThresholds.Item1 == prevThresholds.Item2)
            {
                mixer.SetInputWeight(prevThresholds.Item1, 1f);
                return;
            }

            float normalized = Mathf.InverseLerp(thresholds[prevThresholds.Item1], thresholds[prevThresholds.Item2], parameter);

            mixer.SetInputWeight(prevThresholds.Item1, 1 - normalized);
            mixer.SetInputWeight(prevThresholds.Item2, normalized);
        }

        public void AddMotion(AnimationClip clip, float threshold, float speed = 1f)
        {
            var mixer = (AnimationMixerPlayable)Playable;
            var playable = AnimationClipPlayable.Create(graph, clip);
            playable.SetSpeed(speed);

            playable.SetApplyFootIK(false);
            playable.SetApplyPlayableIK(false);

            int index = thresholds.Count;
            mixer.SetInputCount(index + 1);
            graph.Connect(playable, 0, mixer, index);
            mixer.SetInputWeight(index, 0);

            thresholds.Add(threshold);

            ParameterValueChanged();
        }
    }
}
