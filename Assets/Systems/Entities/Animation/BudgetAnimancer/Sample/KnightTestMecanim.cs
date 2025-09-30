using UnityEngine;
namespace Sample
{
    public class KnightTestMecanim : MonoBehaviour
    {
        [Range(0, 2)] public float Speed;
        void Update()
        {
            GetComponent<Animator>().SetFloat("Speed", Speed);
        }
    }
}