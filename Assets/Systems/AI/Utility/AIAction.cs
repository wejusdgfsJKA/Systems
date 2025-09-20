using UnityEngine;

namespace UtilityAI
{
    public abstract class AIAction : ScriptableObject
    {
        public Consideration Consideration;
        /// <summary>
        /// Returns utility score of action.
        /// </summary>
        /// <param name="context">Agent context.</param>
        /// <returns>Utility score given by consideration.</returns>
        public float Evaluate(Context context)
        {
            return Consideration.Evaluate(context);
        }
        /// <summary>
        /// Sets up anything needed for the action to execute. Called by UilityBrain in Awake.
        /// </summary>
        /// <param name="context">Agent context.</param>
        public virtual void Initialize(Context context) { }
        /// <summary>
        /// Fires when switching to this action.
        /// </summary>
        /// <param name="context">Agent context.</param>
        public virtual void Enter(Context context) { }
        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="context">Agent context.</param>
        /// <param name="deltaTime">Time since last action execution.</param>
        public abstract void Execute(Context context, float deltaTime);
        /// <summary>
        /// Fires when the brain switches to a different action.
        /// </summary>
        /// <param name="context">Agent context.</param>
        public virtual void Exit(Context context) { }
    }
}