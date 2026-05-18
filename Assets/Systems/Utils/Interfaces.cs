using UnityEngine;

public interface IResettable
{
    void PerformReset();
}
public interface IOwnable
{
    Transform Owner { get; set; }
}
