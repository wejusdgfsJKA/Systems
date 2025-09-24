using UnityEngine;
namespace FSM
{
    [System.Serializable]
    public class StateMachine
    {
        [field: SerializeField] public State CurrentState { get; protected set; }
        [field: SerializeField] public State DefaultState { get; protected set; }
        public bool ChangeState(State newState)
        {
            if (newState == null) return false;
            if (CurrentState == null)
            {
                CurrentState = newState;
                CurrentState.Enter();
                return true;
            }
            if (CurrentState.CanChangeState(newState))
            {
                CurrentState.Exit();
                CurrentState = newState;
                CurrentState.Enter();
                return true;
            }
            return false;
        }
        public void Update(float deltaTime)
        {
            CurrentState?.Update(deltaTime);
        }
        public bool ReturnToDefault()
        {
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