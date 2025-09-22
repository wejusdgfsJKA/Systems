using UnityEngine;
using UtilityAI;
public class FighterBrain : UtilityBrain
{
    [field: SerializeField] public Transform Target { get; protected set; }
    protected Transform tr;
    protected override void Awake()
    {
        tr = transform;
        base.Awake();
    }
    protected override void SetupContext()
    {
        base.SetupContext();
        Context.SetData<Transform>(ContextDataKeys.Target, null);
        Context.SetData(ContextDataKeys.DistToTarget, Mathf.Infinity);
    }
    protected override void UpdateContext()
    {
        if (Target != null)
        {
            Context.SetData(ContextDataKeys.DistToTarget, Vector3.Distance(tr.position, Target.position));
        }
    }
    public void SetTarget(Transform target)
    {
        Target = target;
        Context.SetData(ContextDataKeys.Target, target);
        if (target == null) Context.SetData(ContextDataKeys.DistToTarget, Mathf.Infinity);
    }
}
