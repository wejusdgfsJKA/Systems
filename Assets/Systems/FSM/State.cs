using UnityEngine;
namespace FSM
{
    public enum Priority
    {
        Low,
        Medium,
        High
    }
    [System.Serializable]
    public abstract class State
    {
        [field: SerializeField] public Priority Priority { get; protected set; }
        [field: SerializeField] public bool CanInterruptSelf { get; protected set; }
        public abstract void Enter();
        public abstract void Update(float deltaTime);
        public abstract void Exit();
        public bool CanChangeState(State newState)
        {
            if (newState == null) return false;
            if (newState == this && CanInterruptSelf) return true;
            return newState.Priority >= Priority;
        }
    }
    [System.Serializable]
    public class FSM
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
            if (DefaultState == null) return false;
            return ChangeState(DefaultState);
        }
    }
}