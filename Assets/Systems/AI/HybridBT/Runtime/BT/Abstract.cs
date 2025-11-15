using System;
using System.Collections.Generic;
using UnityEngine;

namespace HybridBT
{
    public enum NodeState : byte
    {
        SUCCESS,
        RUNNING,
        FAILURE
    }
    public abstract class BaseElement
    {
        public string Name { get; protected set; }
        public BaseElement(string name)
        {
            Name = name;
        }
    }
    public abstract class BaseElementData : ScriptableObject
    {
        public string Name;
    }
    public abstract class Node<T> : BaseElement
    {
        protected NodeState state = NodeState.FAILURE;
        public NodeState State
        {
            get => state;
            set
            {
                if (state != value)
                {
                    if (value == NodeState.RUNNING)
                    {
                        if (Parent != null) Parent.Abort += Abort;
                    }
                    else if (state == NodeState.RUNNING)
                    {
                        if (Parent != null) Parent.Abort -= Abort;
                        onExit?.Invoke();
                    }
                    state = value;
                }
            }
        }
        public Action Abort;
        protected Node<T> parent;
        public Node<T> Parent
        {
            get => parent;
            set
            {
                parent ??= value;
            }
        }
        protected Action onExit, onEnter;
        protected List<Func<Context<T>, bool>> conditions = new();
        protected List<Action<Context<T>>> services = new();
        public Node(string name, Action onEnter, Action onExit) : base(name)
        {
            this.onEnter = onEnter;
            this.onExit = onExit;
            Abort += () => State = NodeState.FAILURE;
        }
        public virtual void Evaluate(Context<T> context)
        {
            for (var i = 0; i < conditions.Count; i++)
            {
                if (!conditions[i](context))
                {
                    if (State == NodeState.RUNNING) Abort();
                    return;
                }
            }
            for (var i = 0; i < services.Count; i++) services[i](context);
            if (State != NodeState.RUNNING)
            {
                onEnter?.Invoke();
                State = NodeState.RUNNING;
            }
            Execute(context);
        }
        protected abstract void Execute(Context<T> context);
        public void AddService(Action<Context<T>> service)
        {
            if (service == null) throw new ArgumentNullException($"{this} was passed a null service!");
            services.Add(service);
        }
        public void AddCondition(Func<Context<T>, bool> condition)
        {
            if (condition == null) throw new ArgumentNullException($"{this} was passed a null condition!");
            conditions.Add(condition);
        }
    }
    public abstract class NodeData<T> : BaseElementData
    {
        protected virtual Action onEnter { get => null; }
        protected virtual Action onExit { get => null; }
        public List<ConditionData<T>> Conditions = new();
        public List<ServiceData<T>> Services = new();
        protected abstract Node<T> GetNode(Context<T> context);
        public virtual Node<T> ObtainNode(Context<T> context)
        {
            var node = GetNode(context);
            foreach (var item in Conditions) node.AddCondition(item.Func);
            foreach (var item in Services) node.AddService(item.Action);
            return node;
        }
    }
    public abstract class ConditionData<DataKey> : BaseElementData
    {
        public abstract Func<Context<DataKey>, bool> Func { get; }
    }
    public abstract class ServiceData<DataKey> : BaseElementData
    {
        public abstract Action<Context<DataKey>> Action { get; }
    }
}