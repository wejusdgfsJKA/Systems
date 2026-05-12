using System;
using UnityEngine;

namespace HybridBT2
{
    public abstract class Decorator : Node
    {
        protected Node child;
        public Node Child
        {
            get => child;
            set
            {
                child ??= value;
                child.Parent = this;
            }
        }
        protected Decorator(string name, Action<Blackboard> onEnter, Action<Blackboard> onExit) : base(name, onEnter, onExit)
        {

        }
        public override string GetInfo(int indentation)
        {
            return base.GetInfo(indentation) + "\n" + DecoratorInfo(indentation) + '\n' + Child.GetInfo(indentation + 1);
        }
        protected abstract string DecoratorInfo(int indentation);
    }
    public abstract class DecoratorData : NodeData
    {
        public NodeData Child;
        public override Node GetNodeExternal()
        {
            var n = (Decorator)base.GetNodeExternal();
            if (n == null) return null;
            n.Child = Child.GetNodeExternal();
            return n;
        }
    }
    public class Inverter : Decorator
    {
        public Inverter(string name, Action<Blackboard> onEnter, Action<Blackboard> onExit) : base(name, onEnter, onExit)
        {
        }
        protected override string DecoratorInfo(int indentation)
        {
            return "Inverter";
        }
        protected override void ExecuteUnderlyingBehaviour(Blackboard blackboard)
        {
            Child.Evaluate(blackboard);
            if (Child.State == NodeState.SUCCESS) SetState(NodeState.FAILURE, blackboard);
            else if (Child.State == NodeState.FAILURE) SetState(NodeState.SUCCESS, blackboard);
            else SetState(NodeState.RUNNING, blackboard);
        }
    }
    public class TimeLimit : Decorator
    {
        public float MaxTime { get; protected set; }
        public float StartTime { get; protected set; }
        public TimeLimit(string name, float maxTime, Action<Blackboard> onEnter, Action<Blackboard> onExit) : base(name, onEnter, onExit)
        {
            MaxTime = maxTime;
            this.onEnter += (_) => StartTime = 0;
        }
        protected override string DecoratorInfo(int indentation)
        {
            return $"[Time left: {MaxTime - StartTime}]";
        }
        public bool CanExecute()
        {
            return StartTime <= MaxTime;
        }
        protected override void ExecuteUnderlyingBehaviour(Blackboard blackboard)
        {
            if (CanExecute())
            {
                Child.Evaluate(blackboard);
                SetState(Child.State, blackboard);
                StartTime += blackboard.DeltaTime;
            }
            else SetState(NodeState.FAILURE, blackboard);
        }
    }
    public class Repeater : Decorator
    {
        public int MaxNumber = -1;
        public int Count { get; protected set; }
        public Repeater(string name, int maxNumber, Action<Blackboard> onEnter, Action<Blackboard> onExit) : base(name, onEnter, onExit)
        {
            Debug.Assert(maxNumber != 0);
            MaxNumber = maxNumber;
            Count = 0;
            this.onEnter += (ctx) => Count = 0;
        }
        protected override string DecoratorInfo(int indentation)
        {
            return MaxNumber > 0 ? $"[Repeats: {Count}/{MaxNumber}]" : $"[Repeats:{Count}]";
        }

        protected override void ExecuteUnderlyingBehaviour(Blackboard blackboard)
        {
            Child.Evaluate(blackboard);
            if (Child.State != NodeState.FAILURE)
            {
                if (MaxNumber < 0 || Count < MaxNumber)
                {
                    Count++;
                    SetState(NodeState.RUNNING, blackboard);
                }
                else SetState(NodeState.SUCCESS, blackboard);
            }
            else SetState(NodeState.FAILURE, blackboard);
        }
    }
    public class Retry : Decorator
    {
        public int MaxNumber = -1;
        public int Count { get; protected set; }
        public Retry(string name, int maxNumber, Action<Blackboard> onEnter, Action<Blackboard> onExit) : base(name, onEnter, onExit)
        {
            Debug.Assert(maxNumber != 0);
            MaxNumber = maxNumber;
            Count = 0;
            this.onEnter += (ctx) => Count = 0;
        }
        protected override string DecoratorInfo(int indentation)
        {
            return MaxNumber > 0 ? $"[Repeats: {Count}/{MaxNumber}]" : $"[Repeats:{Count}]";
        }

        protected override void ExecuteUnderlyingBehaviour(Blackboard blackboard)
        {
            Child.Evaluate(blackboard);
            if (Child.State != NodeState.SUCCESS)
            {
                if (MaxNumber < 0 || Count < MaxNumber)
                {
                    Count++;
                    SetState(NodeState.RUNNING, blackboard);
                }
                else SetState(NodeState.FAILURE, blackboard);
            }
            else SetState(NodeState.SUCCESS, blackboard);
        }
    }
    public class CooldownDecorator : Decorator
    {
        public float LastTime { get; protected set; }
        public readonly float Cooldown;
        protected bool hasExecuted;
        public CooldownDecorator(string name, float cooldown, Action<Blackboard> onEnter, Action<Blackboard> onExit) : base(name, onEnter, onExit)
        {
            Cooldown = cooldown;
            LastTime = -Cooldown;
            this.onExit ??= delegate { };
            this.onExit += (ctx) =>
            {
                if (hasExecuted)
                {
                    hasExecuted = false;
                    LastTime = Time.time;
                }
            };
        }
        public bool CanExecute()
        {
            return Time.time - LastTime >= Cooldown;
        }
        protected override string DecoratorInfo(int indentation)
        {
            return State == NodeState.RUNNING ? "Operating" : (CanExecute() ? "[Ready]" : $"[{Time.time - LastTime}/{Cooldown}]");
        }

        protected override void ExecuteUnderlyingBehaviour(Blackboard blackboard)
        {
            if (CanExecute())
            {
                hasExecuted = true;
                Child.Evaluate(blackboard);
                SetState(Child.State, blackboard);
            }
            else SetState(NodeState.FAILURE, blackboard);
        }
    }
}