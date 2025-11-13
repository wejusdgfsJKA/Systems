using EventBus;
using System;
using Timers;
namespace Effects
{
    public readonly struct RemoveEffect : IEvent
    {
        public readonly int EffectID;
        public RemoveEffect(int id) => EffectID = id;
    }
    public readonly struct ReceiveHealOverTime : IEvent
    {
        public readonly float Duration;
        public readonly float TickInterval;
        public readonly int HealAmount;
        public readonly int ID;
        public ReceiveHealOverTime(int source, float duration, float tickInterval, int healAmount)
        {
            ID = source;
            Duration = duration;
            TickInterval = tickInterval;
            HealAmount = healAmount;
        }
    }
    public interface IHealable
    {
        void Heal(int amount);
    }
    [Serializable]
    public class HealOverTimeEffect : IEffect<IHealable>
    {
        public float Duration = float.PositiveInfinity;
        public float tickInterval = 1;
        public int HealAmount = 1;
        readonly int id;
        public int ID { get => id; }
        public event Action<IEffect<IHealable>> OnCompleted;

        IntervalTimer timer;
        IHealable currentTarget;
        public HealOverTimeEffect(int source, float duration, float tickInterval, int healAmount)
        {
            id = source;
            Duration = duration;
            this.tickInterval = tickInterval;
            HealAmount = healAmount;
        }

        public void Apply(IHealable target)
        {
            currentTarget = target;
            timer = new(Duration, tickInterval)
            {
                OnInterval = () => currentTarget?.Heal(HealAmount),
                OnTimerStop = OnStop
            };
            timer.Start();
        }
        void OnStop() => Cleanup();

        public void Cancel()
        {
            timer?.Stop();
            Cleanup();
        }

        void Cleanup()
        {
            timer = null;
            currentTarget = null;
            OnCompleted?.Invoke(this);
        }
        public override bool Equals(object obj)
        {
            if (obj is not HealOverTimeEffect other) return false;
            return other.id == id;
        }
        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}