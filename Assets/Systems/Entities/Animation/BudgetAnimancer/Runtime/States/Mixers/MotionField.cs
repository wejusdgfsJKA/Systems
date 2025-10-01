using System;
using UnityEngine;
namespace BudgetAnimancer
{
    [Serializable]
    public class MotionField<T>
    {
        public T Threshold = default;
        public AnimationClip Clip = null;
        public float Speed = 1;
        public MotionField()
        {
            Threshold = default;
            Clip = null;
            Speed = 1;
        }
    }
}