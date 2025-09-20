using Tags;
using UnityEngine;

public class TagDebug : MonoBehaviour
{
    [System.Flags]
    public enum DebugTags
    {
        None = 0,
        Tag0 = 1 << 0,
        Tag1 = 1 << 1,
        Tag2 = 1 << 2,
        All = ~0
    }
    public bool b;
    public DebugTags tags;
    public Transform other;
    private void OnEnable()
    {
        TagManager<DebugTags>.Register(transform, tags);
    }
    private void OnDisable()
    {
        TagManager<DebugTags>.DeRegister(transform);
    }
    private void Update()
    {
        if (b)
        {
            b = false;
            Debug.Log(tags & TagManager<DebugTags>.GetTag(other));
        }
    }
}
