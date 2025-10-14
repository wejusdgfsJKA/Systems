using UnityEngine;
namespace FSM
{
    [System.Serializable]
    public class StateMachine
    {
        /// <summary>
        /// The current state of the FSM.
        /// </summary>
        public State CurrentState { get; protected set; }
        [Tooltip("The default state of the FSM.")]
        [field: SerializeField] public State DefaultState { get; protected set; }
        public StateMachine(State defaultState)
        {
            DefaultState = defaultState;
            CurrentState = defaultState;
        }
        /// <summary>
        /// Switch to a new state. Will succeed if the current state is null or it allows the transition.
        /// See State.CanChangeState. Will call Exit on the old state and Enter on the new State.
        /// </summary>
        /// <param name="newState">The new state to change to.</param>
        /// <returns>True if transition succeeded.</returns>
        public bool ChangeState(State newState)
        {
            if (newState == null)
            {
                Debug.LogError($"New {this} state cannot be null!");
                return false;
            }
            if (CurrentState == null || CurrentState.CanChangeState(newState))
            {
                CurrentState?.Exit();
                CurrentState = newState;
                CurrentState.Enter();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Return to default state. Uses ChangeState or ForceSetState depending on force parameter.
        /// </summary>
        /// <param name="force">If true, will ignore state permissions and use ForceSetState instead of ChangeState.</param>
        /// <returns>True if the transition succeeded.</returns>
        public bool ReturnToDefault(bool force = false)
        {
            if (force) return ChangeState(DefaultState);
            return ForceSetState(DefaultState);
        }
        /// <summary>
        /// Forcefully change to a new state. Will call Exit on the old state and Enter on the new State.
        /// </summary>
        /// <param name="newState">The new state to switch to.</param>
        /// <returns>True if the new state is not null.</returns>
        public bool ForceSetState(State newState)
        {
            if (newState == null)
            {
                Debug.LogError($"New {this} state cannot be null!");
                return false;
            }
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
            return true;
        }
    }
}