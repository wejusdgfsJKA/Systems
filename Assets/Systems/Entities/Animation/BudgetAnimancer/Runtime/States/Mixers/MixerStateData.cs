using System.Collections.Generic;
using UnityEngine;
namespace BudgetAnimancer
{
    public class MixerStateData<T> : ScriptableObject
    {
        [Tooltip("The key for this state. This needs to be different for each state that is used on a layer, " +
            "or the system will not work.")]
        public int Key;
        public List<MotionField<T>> MotionFields = new();
        public T DefaultParamValue = default;
    }
}