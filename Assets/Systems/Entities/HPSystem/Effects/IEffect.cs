using System;
namespace Effects
{
    public interface IEffect<TTarget>
    {
        int ID { get; }
        void Apply(TTarget target);
        void Cancel();
        event Action<IEffect<TTarget>> OnCompleted;
    }
}