using Animation;
using UnityEngine;
namespace Sample
{
    public class StateChanger : MonoBehaviour
    {
        [SerializeField] AnimationController controller;
        public bool b;
        public bool interruptSelf;
        private void Update()
        {
            if (b)
            {
                controller.ChangeAnimatorState(Cube1Hashes.RotateState, canInterruptSelf: interruptSelf);
            }
        }
    }
}