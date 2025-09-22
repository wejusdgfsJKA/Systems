using Animation;
using UnityEngine;

public class StateChanger : MonoBehaviour
{
    [SerializeField] AnimationController controller;
    public bool b;
    public bool interruptSelf;
    private void Update()
    {
        if (b)
        {
            controller.ChangeState(Cube1Hashes.RotateState, canInterruptSelf: interruptSelf);
        }
    }
}
