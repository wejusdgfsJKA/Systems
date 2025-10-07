using System;
using UnityEngine;
using UnityEngine.Playables;
namespace BudgetAnimancer
{
    /// <summary>
    /// Wrapper for a Playable.
    /// </summary>
    public abstract class BudgetAnimancerState
    {
        public Playable Playable { get; protected set; }
        /// <summary>
        /// This state's Mixer index.
        /// </summary>
        public readonly int Index;
        /// <summary>
        /// Default speed of the Playable.
        /// </summary>
        [field: SerializeField] public float Speed { get; set; } = 1;
        /// <summary>
        /// Current speed of the Playable.
        /// </summary>
        public float CurrentSpeed
        {
            get => (float)Playable.GetSpeed();
            set => Playable.SetSpeed(value);
        }
        /// <summary>
        /// Fires when the state is interrupted.
        /// </summary>
        public event Action OnInterrupt;
        public BudgetAnimancerState(Playable playable, int index)
        {
            Playable = playable;
            Index = index;
        }
        /// <summary>
        /// Invokes OnInterrupt.
        /// </summary>
        public virtual void Interrupt()
        {
            OnInterrupt?.Invoke();
        }
        public abstract void Update();
        /// <summary>
        /// Sets CurrentSpeed to default speed.
        /// </summary>
        public virtual void Reset()
        {
            CurrentSpeed = Speed;
        }
    }
}
