using UnityEngine;
namespace FSM
{
    //a byte is more than enough
    public enum Priority : byte
    {
        Low,
        Medium,
        High
    }
    public class State : MonoBehaviour
    {
        [Tooltip("Normally a state can only be replaced by a state with equal/higher priority.")]
        [field: SerializeField] public Priority Priority { get; protected set; }
        [Tooltip("If true, can reenter this state while it is executing.")]
        [field: SerializeField] public bool CanInterruptSelf { get; protected set; }
        /// <summary>
        /// Called when switching to this state.
        /// </summary>
        public virtual void Enter() => enabled = true;
        /// <summary>
        /// Called when switching from this state.
        /// </summary>
        public virtual void Exit() => enabled = false;
        /// <summary>
        /// Returns true if this state can be replaced by a new state.
        /// </summary>
        /// <param name="newState">The new state to switch to.</param>
        /// <returns>True if this state can be replaced by the new state.</returns>
        public bool CanChangeState(State newState)
        {
            if (newState == null)
            {
                Debug.LogError($"{this} cannot be replaced by an empty state!");
                return false;
            }
            if (newState == this && CanInterruptSelf) return true;
            return newState.Priority >= Priority;
        }
    }
}