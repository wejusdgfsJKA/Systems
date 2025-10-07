using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
namespace BudgetAnimancer
{
    /// <summary>
    /// Wrapper for an AnimationClipPlayable.
    /// </summary>
    public class AnimState : BudgetAnimancerState
    {
        public readonly AnimationClip Clip;
        /// <summary>
        /// Returns if the animation clip is looping or not.
        /// </summary>
        public bool Loop
        {
            get => Clip.isLooping;
        }
        /// <summary>
        /// Normalized time of the animation.
        /// </summary>
        public float NormalizedTime
        {
            get => (float)(Playable.GetTime() / Clip.length);
            set => Playable.SetTime(value * Clip.length);
        }
        protected List<(float, Action)> events = new();
        /// <summary>
        /// Fires when the animation reaches its end (if not looping).
        /// </summary>
        public event Action OnEnd;
        protected bool isActive = true;
        float lastTime;
        public AnimState(AnimationClipPlayable playable, AnimationClip clip, int index) : base(playable, index)
        {
            Clip = clip;
        }
        /// <summary>
        /// Add an event to this state.
        /// </summary>
        /// <param name="time">Time to execute (normalized).</param>
        /// <param name="action">Action to execute.</param>
        public void AddEvent(float time, Action action)
        {
            events.Add((time, action));
            events.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        }
        /// <summary>
        /// Check for events to fire.
        /// </summary>
        public override void Update()
        {
            //stop if the state is not active
            if (!isActive) return;
            float currentTime = NormalizedTime;

            // fire normal events
            foreach (var (eventTime, callback) in events)
            {
                if (lastTime < eventTime && currentTime >= eventTime)
                    callback?.Invoke();
            }

            lastTime = currentTime;

            // fire OnEnd for non-looping clips
            if (!Loop && currentTime >= 1)
            {
                OnEnd?.Invoke();
                NormalizedTime = 1;
                CurrentSpeed = 0;
                isActive = false;
            }
        }
        /// <summary>
        /// Sets is active to false. Calls base Interrupt.
        /// </summary>
        public override void Interrupt()
        {
            if (!isActive) return;
            isActive = false;
            base.Interrupt();
        }
        /// <summary>
        /// Sets isActive to true. Sets animation time to 0. Calls base Reset.
        /// </summary>
        public override void Reset()
        {
            isActive = true;
            Playable.SetTime(0);
            base.Reset();
        }
        public override string ToString()
        {
            return Clip.name;
        }
    }
}
