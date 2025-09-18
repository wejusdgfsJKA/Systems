using System.Collections.Generic;
using UnityEngine;
using UtilityAI;

public abstract class UtilityBrain : MonoBehaviour
{
    [field: SerializeField] public List<AIAction> Actions = new();
    protected int currentActionIndex;
    public Context Context { get; protected set; }
    protected virtual void SetupContext()
    {
        Context = new(transform.root);
    }
    protected virtual void Awake()
    {
        SetupContext();
        for (int i = 0; i < Actions.Count; i++)
        {
            Actions[i].Initialize(Context);
        }
    }
    protected virtual void OnEnable()
    {
        currentActionIndex = -1;
    }
    public void Execute(float deltaTime)
    {
        UpdateContext();

        int index = -1;
        float bestUtility = float.MinValue;
        for (int i = 0; i < Actions.Count; i++)
        {
            float utility = Actions[i].Evaluate(Context);
            if (utility > 0 && utility > bestUtility)
            {
                bestUtility = utility;
                index = i;
            }
        }
        if (index != -1)
        {
            if (index != currentActionIndex)
            {
                if (currentActionIndex != -1)
                {
                    Actions[currentActionIndex].Exit(Context);
                }
                currentActionIndex = index;
                Actions[currentActionIndex].Enter(Context);
            }
            Actions[currentActionIndex].Execute(Context, deltaTime);
        }
    }
    protected abstract void UpdateContext();
}
