using System;
using UnityEngine;
using UnityEngine.Playables;
namespace BudgetAnimancer
{
    [Serializable]
    public abstract class BudgetAnimancerState
    {
        public Playable Playable { get; set; }
        public readonly int Index;
        [field: SerializeField] public float Speed { get; set; } = 1;
        public float CurrentSpeed
        {
            get => (float)Playable.GetSpeed();
            set => Playable.SetSpeed(value);
        }
        public event Action OnInterrupt;
        public BudgetAnimancerState(Playable playable, int index)
        {
            Playable = playable;
            Index = index;
        }
        public virtual void Interrupt()
        {
            OnInterrupt?.Invoke();
        }
        public abstract void Update();
        public virtual void Reset()
        {
            CurrentSpeed = Speed;
        }
    }
}
