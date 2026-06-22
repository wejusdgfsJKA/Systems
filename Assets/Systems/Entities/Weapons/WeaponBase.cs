using Spawning2;
using Timers;
using UnityEngine;

namespace Weapons
{
    public class WeaponBase : MonoBehaviour, IResettable
    {
        #region Utils
        /// <summary>
        /// What layers should the weapon BestTarget?
        /// </summary>
        protected static readonly LayerMask targetMask = 1 << 0 | 1 << 6;
        /// <summary>
        /// RaycastHit for targeting.
        /// </summary>
        protected RaycastHit hit;
        /// <summary>
        /// Where the shots are coming from.
        /// </summary>
        [SerializeField] protected Transform shootPoint;

        public bool Firing { get; protected set; }
        #endregion

        #region Weapon parameters
        [SerializeField] protected WeaponParameters @params;
        protected CountdownTimer shotTimer;
        #endregion

        #region Setup
        protected virtual void Awake()
        {
            if (shootPoint == null)
            {
                shootPoint = transform;
            }
            Debug.Assert(@params != null, $"{transform} has no weapon parameters!");
            Debug.Assert(@params.Cooldown >= 0, $"{transform} has negative cooldown!");
            shotTimer = new(@params.Cooldown);
        }
        protected virtual void OnEnable()
        {
            PerformReset();
        }
        /// <summary>
        /// Reset the ammo, cooldown and cast block index of the weapon.
        /// </summary>
        public void PerformReset()
        {
            shotTimer.Reset();
            Firing = false;
        }
        protected void OnDestroy()
        {
            shotTimer.Dispose();
        }
        #endregion

        #region Functionality
        public void StartFiring() => Firing = true;
        public void StopFiring() => Firing = false;
        public virtual void AimAt(Transform target)
        {
            transform.LookAt(target);
        }
        protected void Update()
        {
            if (Firing) Shoot();
        }
        public virtual bool CanShoot()
        {
            return !shotTimer.IsRunning;
        }
        public Bullet Shoot()
        {
            if (!CanShoot()) return null;
            var b = Fire();
            if (b == null) return null;
            shotTimer.Start();
            return b;
        }
        /// <summary>
        /// Fire once.
        /// </summary>
        protected Bullet Fire()
        {
            AdjustShootDirection();
            var b = FireBullet(shootPoint, @params.BulletData);
            return b;
        }
        public Bullet FireBullet(Transform tr, BulletData bulletData)
        {
            if (bulletData == null)
            {
                Debug.LogError($"{transform} received null bullet data!");
                return null;
            }

            var targetRotation = tr.position + tr.forward;
            var b = (Bullet)ObjectPool.TryGetInstance(true).Spawn(bulletData);
            if (b == null)
            {
                Debug.LogError($"{transform} failed to spawn bullet with data {bulletData}!");
                return null;
            }
            b.Owner = transform.root;
            b.transform.position = shootPoint.position;
            b.transform.LookAt(targetRotation);
            b.gameObject.SetActive(true);
            return b;
        }
        /// <summary>
        /// Make the shoot point aim at where the weapon is aiming. Returns imediately if they are the same.
        /// </summary>
        protected void AdjustShootDirection()
        {
            if (shootPoint == transform) return;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100, targetMask))
            {
                shootPoint.LookAt(hit.point);
            }
            else
            {
                shootPoint.LookAt(transform.position + transform.forward * 100);
            }
        }
        #endregion
    }
}