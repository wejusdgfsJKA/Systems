using AudioSystem;
using Spawning2;
using UnityEngine;
namespace Weapons
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Weapons/BulletData", fileName = "BulletData")]
    public class BulletData : PoolableData
    {
        public int Damage;
        public float LifeTime;
        public SoundData SoundData;
    }
}