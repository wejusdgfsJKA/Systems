using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UtilityAI;

public abstract class UtilityBrain : MonoBehaviour
{
    [field: SerializeField] public List<AIActionData> Actions { get; protected set; } = new();
    [field: SerializeField] public Context Context { get; protected set; } = new();
    [field: SerializeField, ReadOnly] public bool ShouldRun { get; set; }
    [SerializeField] protected float updateCooldown = 0.1f;
    #region Don't fuck with this
    protected List<AIAction> actions = new();
    public int CurrentActionIndex { get; protected set; }
    protected Coroutine coroutine;
    /// <summary>
    /// This governs how often the brain updates.
    /// </summary>
    protected WaitForSeconds waitInterval;
    /// <summary>
    /// Secondary condition, used with ShouldRun.
    /// </summary>
    protected WaitUntil waitForPermission;
    #endregion
    #region Setup
    /// <summary>
    /// Self-explanatory. Called in UtilityBrain on Awake.
    /// </summary>
    protected virtual void SetupContext()
    {
        Context.Initialize(transform.root);
    }
    protected virtual void Awake()
    {
        waitInterval = new(updateCooldown);
        waitForPermission = new(() => { return ShouldRun; });
        SetupContext();
        for (int i = 0; i < Actions.Count; i++)
        {
            actions.Add(Actions[i].GetAction(Context));
        }
    }
    protected virtual void OnEnable()
    {
        CurrentActionIndex = -1;
        ShouldRun = true;
        coroutine = StartCoroutine(UpdateLoop());
    }
    protected virtual void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        if (CurrentActionIndex != -1)
        {
            actions[CurrentActionIndex].Exit(Context);
        }
    }
    protected IEnumerator UpdateLoop()
    {
        while (true)
        {
            yield return waitInterval;
            yield return waitForPermission;
            Execute(updateCooldown);
        }
    }
    #endregion
    /// <summary>
    /// Update the context, pick an action and execute it.
    /// </summary>
    /// <param name="deltaTime">Time since last execution.</param>
    public void Execute(float deltaTime)
    {
        UpdateContext();

        int index = -1;
        float bestUtility = float.MinValue;
        for (int i = 0; i < actions.Count; i++)
        {
            float utility = actions[i].Evaluate(Context);
            if (utility > 0 && utility > bestUtility)
            {
                bestUtility = utility;
                index = i;
            }
        }
        if (index != -1)
        {
            if (index != CurrentActionIndex)
            {
                if (CurrentActionIndex != -1)
                {
                    actions[CurrentActionIndex].Exit(Context);
                }
                CurrentActionIndex = index;
                actions[CurrentActionIndex].Enter(Context);
            }
            actions[CurrentActionIndex].Execute(Context, deltaTime);
        }
    }
    protected abstract void UpdateContext();
}
