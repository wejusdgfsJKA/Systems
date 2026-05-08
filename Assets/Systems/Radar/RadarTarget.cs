using UnityEngine;

namespace Radar
{
    public class RadarTarget : MonoBehaviour
    {
        [field: SerializeField] public float RadarSignature { get; protected set; } = 1f;
        protected void OnEnable()
        {
            RadarSystem.Instance.RegisterTarget(this);
        }
        protected void OnDisable()
        {
            RadarSystem.Instance.UnregisterTarget(this);
        }
    }
}