using UnityEngine;

namespace UtilityAI
{
    public abstract class Consideration : ScriptableObject
    {
        /// <summary>
        /// Returns a utility value between 0 and 1 for a given Context.
        /// </summary>
        /// <param name="context">Agent Context.</param>
        /// <returns>Utility value of consideration.</returns>
        public abstract float Evaluate(Context context);
    }
}