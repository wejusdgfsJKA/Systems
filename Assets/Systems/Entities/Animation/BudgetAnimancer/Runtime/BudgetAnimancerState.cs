using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
namespace BudgetAnimancer
{
    public class BudgetAnimancerState
    {
        public readonly AnimationClipPlayable Playable;
        public readonly AnimationClip Clip;
        public bool Loop
        {
            get => Clip.isLooping;
        }
        public float NormalizedTime
        {
            get => (float)(Playable.GetTime() / Playable.GetAnimationClip().length);
            set => Playable.SetTime(value * Playable.GetAnimationClip().length);
        }
        public float Length => Clip.length;
        public readonly int Index;
        protected List<(float, Action)> events = new();
        public event Action OnEnd, OnInterrupt;
        float lastTime;
        public float Time
        {
            get => (float)Playable.GetTime();
            set => Playable.SetTime(value);
        }
        public float Speed
        {
            get => (float)Playable.GetSpeed();
            set => Playable.SetSpeed(value);
        }
        public bool IsActive { get; set; } = true;
        public BudgetAnimancerState(AnimationClipPlayable playable, AnimationClip clip, int index)
        {
            Playable = playable;
            Clip = clip;
            Index = index;
        }
        public void AddEvent(float time, Action action)
        {
            events.Add((time, action));
            events.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        }
        public void CheckEvents()
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
            }
        }
        public void TriggerInterrupt()
        {
            if (!IsActive) return;
            IsActive = false;
            OnInterrupt?.Invoke();
        }
        public override string ToString()
        {
            return Clip.name;
        }
    }
}
