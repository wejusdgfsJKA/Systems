using UnityEngine;

namespace Radar
{
    public class RadarTarget : MonoBehaviour
    {
        [field: SerializeField] public float RadarSignature { get; protected set; } = 1f;
        protected void OnEnable()
        {
            RadarSystem.TryGetInstance(true).RegisterTarget(this);
        }
        protected void OnDisable()
        {
            RadarSystem.TryGetInstance().UnregisterTarget(this);
        }
    }
}