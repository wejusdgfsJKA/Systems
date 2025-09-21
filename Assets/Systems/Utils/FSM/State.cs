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
}