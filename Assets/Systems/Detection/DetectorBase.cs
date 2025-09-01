using System.Collections;
using UnityEngine;
namespace Detection
{
    public abstract class DetectorBase : MonoBehaviour
    {
        /// <summary>
        /// Cooldown between checks.
        /// </summary>
        public float Cooldown { get; set; }
        protected WaitForSeconds waitForCooldown;
        protected Coroutine mainCoroutine;
        protected virtual void Awake()
        {
            waitForCooldown = new WaitForSeconds(Cooldown);
        }
        protected virtual void OnEnable()
        {
            mainCoroutine = StartCoroutine(Enumerator());
        }
        protected virtual IEnumerator Enumerator()
        {
            while (true)
            {
                yield return waitForCooldown;
                HandleDetection();
            }
        }
        protected abstract void HandleDetection();
        protected virtual void OnDisable()
        {
            if (mainCoroutine != null)
            {
                StopCoroutine(mainCoroutine);
            }
        }
    }
}