using System.Collections.Generic;
using UnityEngine;
namespace BudgetAnimancer
{
    public class MixerStateData<T> : ScriptableObject
    {
        public List<MotionField<T>> MotionFields = new();
        public T DefaultParamValue = default;
    }
}