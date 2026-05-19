using UnityEngine;
namespace Weapons
{
    /// <summary>
    /// Parameters of a weapon.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Weapons/WeaponParameters", fileName = "WeaponParameters")]
    [System.Serializable]
    public class WeaponParameters : ScriptableObject
    {
        [Tooltip("cooldown between shots.")]
        public float Cooldown;
        public BulletData BulletData;
    }
}