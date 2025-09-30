using System;
using UnityEngine;
namespace BudgetAnimancer
{
    [Serializable]
    public struct MotionField<T>
    {
        public T Threshold;
        public AnimationClip Clip;
        public float Speed;
        public MotionField(T threshold, AnimationClip clip, float speed)
        {
            Threshold = threshold;
            Clip = clip;
            Speed = speed;
        }
    }
}