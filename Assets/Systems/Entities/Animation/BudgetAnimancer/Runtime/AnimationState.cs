using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
namespace BudgetAnimancer
{
    public class AnimationState : BudgetAnimancerState
    {
        public readonly AnimationClip Clip;
        public bool Loop
        {
            get => Clip.isLooping;
        }
        public float NormalizedTime
        {
            get => (float)(Playable.GetTime() / Clip.length);
            set => Playable.SetTime(value * Clip.length);
        }
        protected List<(float, Action)> events = new();
        public event Action OnEnd;
        public bool IsActive { get; set; } = true;
        float lastTime;
        public AnimationState(AnimationClipPlayable playable, AnimationClip clip, int index) : base(playable, index)
        {
            Clip = clip;
        }
        public void AddEvent(float time, Action action)
        {
            events.Add((time, action));
            events.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        }
        public override void Update()
        {
            if (!IsActive) return;
            float currentTime = NormalizedTime;

            // Fire normal events
            foreach (var (eventTime, callback) in events)
            {
                if (lastTime < eventTime && currentTime >= eventTime)
                    callback?.Invoke();
            }

            lastTime = currentTime;

            // Fire OnEnd for non-looping clips
            if (!Loop && currentTime >= 1)
            {
                OnEnd?.Invoke();
                NormalizedTime = 1;
                CurrentSpeed = 0;
                IsActive = false;
            }
        }
        public override void Interrupt()
        {
            if (!IsActive) return;
            IsActive = false;
            base.Interrupt();
        }
        public override void Reset()
        {
            IsActive = true;
            Playable.SetTime(0);
            base.Reset();
        }
        public override string ToString()
        {
            return Clip.name + " " + NormalizedTime;
        }
    }
}
