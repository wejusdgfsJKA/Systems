using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Detection2
{
    public abstract class TargetEvaluationStrategy<T> : ScriptableObject where T : class, IDetectionTarget
    {
        public abstract float Evaluate(DetectorBase<T> detector, Transform tr);
        public abstract float Evaluate(DetectorBase<T> detector, T target);
    }
    public interface IDetectionTarget
    {
        public float Score { get; set; }
        public Transform Transform { get; }
    }
    public abstract class DetectorBase<T> : MonoBehaviour where T : class, IDetectionTarget
    {
        #region Detection paremeters
        [Header("Detection Parameters")]
        [SerializeField] protected Transform eye;
        public Transform Eye => eye != null ? eye : Transform;
        /// <summary>
        /// cooldown between checks.
        /// </summary>
        [SerializeField] protected float cooldown = .1f;
        [SerializeField] protected LayerMask targetMask, obstructionMask = 1 << 0;
        [Tooltip("Radius of the detection sphere.")]
        [SerializeField] protected float sightRange = 50;
        [Tooltip("Field of view angle in degrees. 360 means no field of view, i.e., the detector can see in all directions.")]
        [SerializeField] protected float fieldOfViewAngle = 360;
        [Tooltip("Range within which the target is considered close enough to be detected without checking for line of sight or field of view.")]
        [SerializeField] protected float proximityRange = 1;
        #endregion

        #region Helpers        
        protected HashSet<int> targetSet = new();
        protected Transform Transform;
        protected WaitForSeconds waitForCooldown;
        protected Coroutine mainCoroutine;
        protected Collider[] colliders = new Collider[20];
        [SerializeField] protected TargetEvaluationStrategy<T> evaluationStrategy;
        public UnityEvent<T> TargetChanged;
        protected T target;
        public T BestTarget
        {
            get
            {
                return target;
            }
            protected set
            {
                if (!Equals(target, value))
                {
                    target = value;
                    TargetChanged?.Invoke(target);
                }
            }
        }
        public void ResetTarget()
        {
            BestTarget = null;
        }
        protected virtual bool IsInProximity(Vector3 pos)
        {
            return Vector3.Distance(transform.position, pos) <= proximityRange;
        }
        protected float cachedCosValue = 0;
        protected virtual bool CanSee(Vector3 pos)
        {
            if (fieldOfViewAngle < 360 && Vector3.Dot((pos - Eye.position).normalized, Eye.forward) < cachedCosValue)
            {
                return false;
            }
            if (obstructionMask != 0 && Physics.Linecast(Eye.position, pos, obstructionMask))
            {
                return false;
            }
            return true;
        }
        protected abstract T Convert(Transform tr);
        protected virtual float TargetScore(Transform tr)
        {
            return evaluationStrategy != null ? evaluationStrategy.Evaluate(this, tr) : 1;
        }
        protected virtual float TargetScore(T target)
        {
            return evaluationStrategy != null ? evaluationStrategy.Evaluate(this, target) : 1;
        }
        #endregion

        #region Setup
        protected virtual void Awake()
        {
            Transform = transform;
            waitForCooldown = new WaitForSeconds(cooldown);
        }
        protected virtual void OnEnable()
        {
            cachedCosValue = Mathf.Cos(Mathf.Deg2Rad * fieldOfViewAngle / 2);
            mainCoroutine = StartCoroutine(DetectionEnumerator());
            ResetTarget();
        }
        protected virtual IEnumerator DetectionEnumerator()
        {
            while (true)
            {
                yield return waitForCooldown;
                HandleDetection();
            }
        }
        protected virtual void OnDisable()
        {
            if (mainCoroutine != null)
            {
                StopCoroutine(mainCoroutine);
            }
        }
        protected virtual void OnDestroy()
        {
            TargetChanged = null;
            BestTarget = null;
        }
        #endregion

        #region Functionality
        protected virtual void HandleDetection()
        {
            DetectNewTargets();
        }
        protected virtual void DetectNewTargets()
        {
            targetSet.Add(transform.root.GetInstanceID());
            int nrOfTargets = Physics.OverlapSphereNonAlloc(transform.position, sightRange, colliders, targetMask);
            for (int i = 0; i < nrOfTargets; i++)
            {
                var tr = colliders[i].transform.root;
                if (targetSet.Contains(tr.GetInstanceID())) continue;
                if (IsInProximity(tr.position) || CanSee(tr.position))
                {
                    targetSet.Add(tr.GetInstanceID());
                    ProcessNewTarget(tr);
                }
            }
            targetSet.Clear();
        }
        protected abstract void ProcessNewTarget(Transform tr);
        #endregion
    }
}