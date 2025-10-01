using System;
using UnityEngine;
namespace FSM
{
    public enum Priority : byte
    {
        Low,
        Medium,
        High
    }
    [Serializable]
    public class State : MonoBehaviour
    {
        [field: SerializeField] public Priority Priority { get; protected set; }
        [field: SerializeField] public bool CanInterruptSelf { get; protected set; }
        public virtual void Enter() => enabled = true;
        public virtual void Exit() => enabled = false;
        public bool CanChangeState(State newState)
        {
            if (newState == null) return false;
            if (newState == this && CanInterruptSelf) return true;
            return newState.Priority >= Priority;
        }
    }
}