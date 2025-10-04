using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace BudgetAnimancer
{
    public class Mixer2DState : MixerState
    {
        protected Vector2 parameter;
        public Vector2 Parameter
        {
            get => parameter;
            set
            {
                if (parameter != value)
                {
                    parameter = value;
                    parameter = Vector2.ClampMagnitude(parameter, 1);
                    ParameterValueChanged();
                }
            }
        }
        protected readonly List<Vector2> thresholds = new();
        public Mixer2DState(PlayableGraph graph, List<(Vector2 threshold, AnimationClip clip, float speed)> motionFields, int index) : base(graph, index)
        {
            var mixer = (AnimationMixerPlayable)Playable;
            mixer.SetInputCount(motionFields.Count);

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

            ParameterValueChanged();
        }

        public Mixer2DState(PlayableGraph graph, int index, Mixer2DStateData data) : base(graph, index)
        {
            var motionFields = data.MotionFields;
            var mixer = (AnimationMixerPlayable)Playable;
            mixer.SetInputCount(motionFields.Count);

            for (int i = 0; i < motionFields.Count; i++)
            {
                var (threshold, clip, speed) = (motionFields[i].Threshold, motionFields[i].Clip, motionFields[i].Speed);
                var playable = AnimationClipPlayable.Create(graph, clip);
                playable.SetSpeed(speed);

                // Prevent IK/Root scaling artifacts
                playable.SetApplyFootIK(false);
                playable.SetApplyPlayableIK(false);

                graph.Connect(playable, 0, mixer, i);
                mixer.SetInputWeight(i, 0);

                thresholds.Add(threshold);
            }
            parameter = data.DefaultParamValue;
            ParameterValueChanged();
        }

        protected override void ParameterValueChanged()
        {
            if (thresholds.Count < 2) return;

            // Compute weights using inverse distance weighting (simple barycentric approximation)
            float totalWeight = 0f;
            float[] weights = new float[thresholds.Count];
            var mainMagnitude = parameter.magnitude;
            for (int i = 0; i < thresholds.Count; i++)
            {
                var magnitude = (thresholds[i] - parameter).magnitude;
                if (magnitude <= mainMagnitude)
                {
                    float weight = 1f / Mathf.Max(magnitude, 0.0001f); // Avoid division by zero
                    weights[i] = weight;
                    totalWeight += weight;
                }
                else
                {
                    weights[i] = 0;
                }
            }

            // Normalize weights and apply them to mixer
            for (int i = 0; i < thresholds.Count; i++)
            {
                Playable.SetInputWeight(i, weights[i] / totalWeight);
            }
        }
    }
}
