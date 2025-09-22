using UnityEngine;
namespace Sample
{
    public class DebugLogger : MonoBehaviour
    {
        public void Debug1()
        {
            Debug.Log($"{transform} received event with ID 1.");
        }
        public void Debug2()
        {
            Debug.Log($"{transform} received event with ID 2.");
        }
        public void Debug3()
        {
            Debug.Log($"{transform} received event with ID 3.");
        }
    }
}