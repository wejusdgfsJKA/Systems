using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace BudgetAnimancer
{
    public class Layer
    {
        public AnimationMixerPlayable Mixer { get; protected set; }
        protected PlayableGraph graph;
        public Dictionary<object, BudgetAnimancerState> StateCache { get; protected set; } = new();
        private float blendDuration;
        private float blendTime;
        public BudgetAnimancerState CurrentState { get; protected set; }
        protected HashSet<int> fadeouts = new();
        public Layer(PlayableGraph playableGraph)
        {
            graph = playableGraph;
            Mixer = AnimationMixerPlayable.Create(playableGraph);
        }

        #region States
        #region AnimationStates
        public AnimState Play(AnimationClip clip, float duration = 0.25f)
        {
            var state = CreateOrGetAnimationState(clip);
            StartBlend(state, duration);
            return state;
        }

        public AnimState CreateOrGetAnimationState(AnimationClip clip)
        {
            if (!StateCache.TryGetValue(clip, out var state))
            {
                int newIndex = Mixer.GetInputCount();
                state = new AnimState(AnimationClipPlayable.Create(graph, clip), clip, newIndex);
                StateCache.Add(clip, state);

                Mixer.SetInputCount(newIndex + 1);
                graph.Connect(state.Playable, 0, Mixer, newIndex);
                Mixer.SetInputWeight(newIndex, 0f); // start at 0 weight
            }
            return (AnimState)state;
        }
        #endregion

        #region Linear mixers
        public LinearMixerState PlayLinearMixer(object key, float blendDuration = 0.25f)
        {
            if (StateCache.TryGetValue(key, out var state))
            {
                StartBlend(state, blendDuration);
                return (LinearMixerState)state;
            }
            return null;
        }

        public LinearMixerState PlayLinearMixer(object key, List<(float, AnimationClip, float)> motionFields, float blendDuration = 0.25f, float initialParameterValue = 0)
        {
            var state = GetOrAddLinearMixer(key, motionFields, initialParameterValue);
            StartBlend(state, blendDuration);
            return state;
        }

        public LinearMixerState GetOrAddLinearMixer(object key, List<(float, AnimationClip, float)> motionFields, float initialParameterValue = 0)
        {
            if (!StateCache.TryGetValue(key, out var state))
            {
                int newIndex = Mixer.GetInputCount();
                state = new LinearMixerState(graph, newIndex, motionFields, initialParameterValue);
                StateCache.Add(key, state);

                Mixer.SetInputCount(newIndex + 1);
                graph.Connect(state.Playable, 0, Mixer, newIndex);
                Mixer.SetInputWeight(newIndex, 0f); // start at 0 weight
            }
            return (LinearMixerState)state;
        }

        public LinearMixerState GetOrAddLinearMixer(object key, List<MotionField<float>> motionFields, float initialParameterValue = 0)
        {
            if (!StateCache.TryGetValue(key, out var state))
            {
                int newIndex = Mixer.GetInputCount();
                state = new LinearMixerState(graph, newIndex, motionFields, initialParameterValue);
                StateCache.Add(key, state);

                Mixer.SetInputCount(newIndex + 1);
                graph.Connect(state.Playable, 0, Mixer, newIndex);
                Mixer.SetInputWeight(newIndex, 0f); // start at 0 weight
            }
            return (LinearMixerState)state;
        }

        public LinearMixerState PlayLinearMixer(LinearMixerStateData data, float blendDuration = 0.25f)
        {
            var state = GetOrAddLinearMixer(data);
            StartBlend(state, blendDuration);
            return state;
        }
        public LinearMixerState GetOrAddLinearMixer(LinearMixerStateData data)
        {
            if (!StateCache.TryGetValue(data.Key, out var state))
            {
                int newIndex = Mixer.GetInputCount();
                state = new LinearMixerState(graph, newIndex, data);
                StateCache.Add(data.Key, state);

                Mixer.SetInputCount(newIndex + 1);
                graph.Connect(state.Playable, 0, Mixer, newIndex);
                Mixer.SetInputWeight(newIndex, 0f); // start at 0 weight
            }
            return (LinearMixerState)state;
        }
        #endregion
        #endregion

        #region Blending & updates
        private void StartBlend(BudgetAnimancerState nextState, float duration)
        {
            if (CurrentState == nextState) return;
            if (CurrentState != null)
            {
                // Previous animation is being interrupted
                CurrentState.Interrupt();
                fadeouts.Add(CurrentState.Index);
            }
            CurrentState = nextState;
            blendDuration = duration;
            blendTime = 0f;

            CurrentState.Reset();
        }

        protected void HandleBlend(float deltaTime)
        {
            if (CurrentState == null) return;
            // If no previous attackState, just make current fully active
            if (fadeouts.Count == 0)
            {
                Mixer.SetInputWeight(CurrentState.Index, 1f);
                return;
            }

            // Update blend
            blendTime += deltaTime;
            float t = blendDuration > 0f ? Mathf.Clamp01(blendTime / blendDuration) : 1f;

            // Blend finished
            if (t >= 1f)
            {
                foreach (var index in fadeouts)
                {
                    Mixer.SetInputWeight(index, 0);
                }
                fadeouts.Clear();
                Mixer.SetInputWeight(CurrentState.Index, 1f);
                return;
            }

            foreach (var index in fadeouts.ToList())
            {
                var weight = 1 - t;
                if (weight <= 0)
                {
                    Mixer.SetInputWeight(index, 0);
                    fadeouts.Remove(index);
                }
                else
                {
                    Mixer.SetInputWeight(index, weight);
                }
            }
            Mixer.SetInputWeight(CurrentState.Index, t);
        }

        public void Update(float deltaTime)
        {
            HandleBlend(deltaTime);
            CurrentState?.Update();
        }
        #endregion
    }
}