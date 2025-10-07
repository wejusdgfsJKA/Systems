using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace BudgetAnimancer
{
    /// <summary>
    /// Controls an animation layer.
    /// </summary>
    public class Layer
    {
        public AnimationMixerPlayable Mixer { get; protected set; }
        protected PlayableGraph graph;
        public Dictionary<object, BudgetAnimancerState> StateCache { get; } = new();
        //don't mess with this
        private float blendDuration;
        private float blendTime;
        public BudgetAnimancerState CurrentState { get; protected set; }
        /// <summary>
        /// Holds all states with above zero weight other than the current state, to avoid ghost weights.
        /// If we only held on to the previous state,rapid transitions could cause certain states' weights
        /// to not be fully reduced to 0.
        /// </summary>
        protected HashSet<int> fadeouts = new();
        public Layer(PlayableGraph playableGraph)
        {
            graph = playableGraph;
            Mixer = AnimationMixerPlayable.Create(playableGraph);
        }

        #region States
        #region AnimationStates
        /// <summary>
        /// Play an animation on this layer. Calls CreateOrGetAnimationState, then start blend.
        /// </summary>
        /// <param name="clip">Animation clip to play.</param>
        /// <param name="blendDuration">How long should blending take.</param>
        /// <returns>The corresponding animation state.</returns>
        public AnimState Play(AnimationClip clip, float blendDuration = 0.25f)
        {
            if (clip == null)
            {
                Debug.LogError($"{this} cannot play empty clip!");
            }
            var state = CreateOrGetAnimationState(clip);
            StartBlend(state, blendDuration);
            return state;
        }

        /// <summary>
        /// Create (if necessary) and return an animation state for a clip on this layer.
        /// </summary>
        /// <param name="clip">Animation clip.</param>
        /// <returns>The animation state.</returns>
        public AnimState CreateOrGetAnimationState(AnimationClip clip)
        {
            if (clip == null)
            {
                Debug.LogError("Cannot create animation state from null clip!");
                return null;
            }
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

        #region 2D mixers
        public LinearMixerState Play2DMixer(object key, float blendDuration = 0.25f)
        {
            if (StateCache.TryGetValue(key, out var state))
            {
                StartBlend(state, blendDuration);
                return (LinearMixerState)state;
            }
            return null;
        }

        public Mixer2DState PlayMixer2D(Mixer2DStateData data, float blendDuration = 0.25f)
        {
            var state = GetOrAddMixer2D(data);
            StartBlend(state, blendDuration);
            return state;
        }
        public Mixer2DState GetOrAddMixer2D(Mixer2DStateData data)
        {
            if (!StateCache.TryGetValue(data.Key, out var state))
            {
                int newIndex = Mixer.GetInputCount();
                state = new Mixer2DState(graph, newIndex, data);
                StateCache.Add(data.Key, state);

                Mixer.SetInputCount(newIndex + 1);
                graph.Connect(state.Playable, 0, Mixer, newIndex);
                Mixer.SetInputWeight(newIndex, 0f); // start at 0 weight
            }
            return (Mixer2DState)state;
        }
        #endregion
        #endregion

        #region Blending & updates
        /// <summary>
        /// Begin blending to a new state. Calls Interrupt on the old state if it exists and calls Reset 
        /// on the new state. Removes the next state from fadeouts set.
        /// </summary>
        /// <param name="nextState">The next state to switch to. Nothing happens if this is null.</param>
        /// <param name="blendDuration">How long should the blend take.</param>
        private void StartBlend(BudgetAnimancerState nextState, float blendDuration)
        {
            if (nextState == null)
            {
                Debug.LogError($"{this} cannot blend to an empty state!");
                return;
            }
            fadeouts.Remove(nextState.Index);
            if (CurrentState == nextState) return;
            if (CurrentState != null)
            {
                // Previous animation is being interrupted
                CurrentState.Interrupt();
                fadeouts.Add(CurrentState.Index);
            }
            CurrentState = nextState;
            this.blendDuration = blendDuration;
            blendTime = 0f;

            CurrentState.Reset();

            if (fadeouts.Count == 0)
            {
                Mixer.SetInputWeight(CurrentState.Index, 1);
            }
        }

        /// <summary>
        /// Blend from the old state(s) to a new one. Returns if the CurrentState is null or if the 
        /// fadeouts hashset is empty. Otherwise slowly decreases the weights of all states in the hashset, 
        /// while increasing the weight of the current state. All states in fadeouts which have weight 0 are
        /// removed from the set.
        /// </summary>
        /// <param name="deltaTime">Time since last call.</param>
        protected void HandleBlend(float deltaTime)
        {
            if (CurrentState == null) return;

            if (fadeouts.Count == 0) return;

            // Update blend
            blendTime += deltaTime;
            float t = blendDuration > 0f ? Mathf.Clamp01(blendTime / blendDuration) : 1f;

            //decrease the weights of all previous states
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

        /// <summary>
        /// Calls HandleBlend and Update on the current state.
        /// </summary>
        /// <param name="deltaTime">Time since last call.</param>
        public void Update(float deltaTime)
        {
            HandleBlend(deltaTime);
            CurrentState?.Update();
        }
        #endregion
    }
}