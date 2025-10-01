using UnityEngine;
namespace FSM
{
    [System.Serializable]
    public class StateMachine
    {
        [field: SerializeField] public State CurrentState { get; protected set; }
        [field: SerializeField] public State DefaultState { get; protected set; }
        public StateMachine(State defaultState)
        {
            DefaultState = defaultState;
            CurrentState = defaultState;
        }
        public bool ChangeState(State newState)
        {
            if (newState == null) return false;
            if (CurrentState == null || CurrentState.CanChangeState(newState))
            {
                CurrentState?.Exit();
                CurrentState = newState;
                CurrentState.Enter();
                return true;
            }
            return false;
        }
        public bool ReturnToDefault(bool force = false)
        {
            if (force) return ChangeState(DefaultState);
            return ForceSetState(DefaultState);
        }
        public bool ForceSetState(State newState)
        {
            if (newState == null) return false;
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
            return true;
        }
    }
}