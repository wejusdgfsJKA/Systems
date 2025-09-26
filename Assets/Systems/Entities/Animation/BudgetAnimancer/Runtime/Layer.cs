using System.Collections.Generic;
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
        private BudgetAnimancerState previousState;
        private float blendDuration;
        private float blendTime;
        public BudgetAnimancerState CurrentState { get; protected set; }
        public Layer(PlayableGraph playableGraph)
        {
            graph = playableGraph;
            Mixer = AnimationMixerPlayable.Create(playableGraph);
        }

        public BudgetAnimancerState Play(AnimationClip clip, float duration = 0.25f)
        {
            var state = CreateOrGetState(clip);
            StartBlend(state, duration);
            return state;
        }

        public BudgetAnimancerState CreateOrGetState(AnimationClip clip)
        {
            if (!StateCache.TryGetValue(clip, out var state))
            {
                int newIndex = Mixer.GetInputCount();
                state = new BudgetAnimancerState(AnimationClipPlayable.Create(graph, clip), clip, newIndex);
                StateCache.Add(clip, state);

                Mixer.SetInputCount(newIndex + 1);
                graph.Connect(state.Playable, 0, Mixer, newIndex);
                Mixer.SetInputWeight(newIndex, 0f); // start at 0 weight
            }
            return state;
        }

        private void StartBlend(BudgetAnimancerState nextState, float duration)
        {
            if (CurrentState == nextState) return;
            if (CurrentState != null)
            {
                // Previous animation is being interrupted
                CurrentState.TriggerInterrupt();
            }
            previousState = CurrentState;
            CurrentState = nextState;
            blendDuration = duration;
            blendTime = 0f;

            CurrentState.Time = 0f;
            CurrentState.Speed = 1f;
            CurrentState.IsActive = true;
        }

        protected void HandleBlend(float deltaTime)
        {
            if (CurrentState == null) return;

            // If no previous state, just make current fully active
            if (previousState == null)
            {
                Mixer.SetInputWeight(CurrentState.Index, 1f);
                return;
            }

            // Update blend
            blendTime += deltaTime;
            float t = blendDuration > 0f ? Mathf.Clamp01(blendTime / blendDuration) : 1f;

            // Apply weights
            Mixer.SetInputWeight(previousState.Index, 1f - t);
            Mixer.SetInputWeight(CurrentState.Index, t);

            // Blend finished
            if (t >= 1f)
            {
                Mixer.SetInputWeight(previousState.Index, 0f);
                previousState = null;
                Mixer.SetInputWeight(CurrentState.Index, 1f);
            }
        }

        public void Update(float deltaTime)
        {
            HandleBlend(deltaTime);
            HandleCurrentState();
        }

        protected void HandleCurrentState()
        {
            if (CurrentState == null) return;


            // Fire events only if the state is active
            CurrentState.CheckEvents();

            // Stop if non-looping and reached end
            if (CurrentState.IsActive && !CurrentState.Loop &&
                (float)CurrentState.Playable.GetTime() >= CurrentState.Clip.length)
            {
                CurrentState.Speed = 0f;          // stop playback
                CurrentState.IsActive = false;
            }
        }
    }
}