using AudioSystem;
using EventBus;
using HP;
using Spawning2;
using Timers;
using UnityEngine;
using Utilities;
namespace Weapons
{
    public class Bullet : Poolable, IOwnable
    {
        protected TakeDamage takeDamage = new();
        public Transform Owner
        {
            get
            {
                if (takeDamage.Source == null)
                {
                    return null;
                }
                return takeDamage.Source;
            }
            set
            {
                takeDamage.Source = value;
            }
        }
        protected Transform tr;
        protected CountdownTimer lifeTimeTimer;
        protected SoundData soundData;
        protected virtual void Awake()
        {
            ComponentRegister<IOwnable>.Register(tr, this);
            tr = transform;
            lifeTimeTimer = new CountdownTimer(0);
            lifeTimeTimer.OnTimerStop += () => gameObject.SetActive(false);
        }
        protected virtual void OnEnable()
        {
            SoundManager.Instance.Play(soundData, tr.position);
            lifeTimeTimer.Start();
        }
        public override void Initialize(PoolableData poolableData)
        {
            base.Initialize(poolableData);
            if (poolableData is not BulletData bulletData)
            {
                throw new System.ArgumentException($"Expected {typeof(BulletData)}, got {poolableData.GetType()}");
            }
            takeDamage.Damage = bulletData.Damage;
            lifeTimeTimer.Reset(bulletData.LifeTime);
            soundData = bulletData.SoundData;
        }
        protected virtual void OnDestroy()
        {
            ComponentRegister<IOwnable>.Unregister(tr);
            lifeTimeTimer.Dispose();
        }
        protected override void OnDisable()
        {
            lifeTimeTimer.Stop();
            base.OnDisable();
        }
        protected virtual void OnHit(Collider collider)
        {
            takeDamage.ColliderID = collider.GetInstanceID();
            EventBus<TakeDamage>.Raise(collider.transform.root.GetInstanceID(), takeDamage);
        }
    }
}