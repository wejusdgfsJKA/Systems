using UnityEngine;
namespace Weapons.Melee
{
    [System.Serializable]
    public struct MeleeHitbox
    {
        [field: SerializeField] public float Radius { get; private set; }
        [field: SerializeField] public Vector3 Offset { get; private set; }
    }
}