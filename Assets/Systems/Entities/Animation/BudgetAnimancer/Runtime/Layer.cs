using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace BudgetAnimancer
{
    public class Layer
    {
        public AnimationMixerPlayable mixer { get; protected set; }
        protected PlayableGraph graph;
        public Dictionary<object, AnimationState> stateCache { get; protected set; } = new();

        private AnimationState previousState, currentState;
        private float blendDuration;
        private float blendTime;

        public Layer(PlayableGraph playableGraph)
        {
            graph = playableGraph;
            mixer = AnimationMixerPlayable.Create(playableGraph);
        }

        public AnimationState Play(AnimationClip clip, float duration = 0.25f)
        {
            if (!stateCache.TryGetValue(clip, out var state))
            {
                int newIndex = mixer.GetInputCount();
                state = new AnimationState(AnimationClipPlayable.Create(graph, clip), clip, newIndex);
                stateCache.Add(clip, state);

                mixer.SetInputCount(newIndex + 1);
                graph.Connect(state.Playable, 0, mixer, newIndex);
                mixer.SetInputWeight(newIndex, 0f); // start at 0 weight
            }

            StartBlend(state, duration);
            return state;
        }

        private void StartBlend(AnimationState nextState, float duration)
        {
            if (currentState != null && currentState != nextState)
            {
                // Previous animation is being interrupted
                currentState.TriggerInterrupt();
            }
            previousState = currentState;
            currentState = nextState;
            blendDuration = duration;
            blendTime = 0f;

            currentState.Time = 0f;
            currentState.Weight = 0f;
            currentState.Speed = 1f;
        }
        protected void HandleBlend(float deltaTime)
        {
            if (currentState == null) return;

            // If no previous state, just make current fully active
            if (previousState == null)
            {
                mixer.SetInputWeight(currentState.Index, 1f);
                return;
            }

            // Update blend
            blendTime += deltaTime;
            float t = blendDuration > 0f ? Mathf.Clamp01(blendTime / blendDuration) : 1f;

            // Apply weights
            mixer.SetInputWeight(previousState.Index, 1f - t);
            mixer.SetInputWeight(currentState.Index, t);

            // Blend finished
            if (t >= 1f)
            {
                mixer.SetInputWeight(previousState.Index, 0f);
                previousState = null;
                mixer.SetInputWeight(currentState.Index, 1f);
            }
        }

        public void Update(float deltaTime)
        {
            HandleBlend(deltaTime);
            HandleCurrentState();
        }

        protected void HandleCurrentState()
        {
            if (currentState == null) return;


            // Fire events only if the state is active
            currentState.CheckEvents();

            // Stop if non-looping and reached end
            if (currentState.IsActive && !currentState.Loop &&
                (float)currentState.Playable.GetTime() >= currentState.Clip.length)
            {
                currentState.Speed = 0f;          // stop playback
                currentState.IsActive = false;
            }
        }
    }
}