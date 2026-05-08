using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Radar
{
    public class RadarSystem : Singleton<RadarSystem>
    {
        public RectTransform center;
        public float radius = 2;
        [SerializeField] GameObject radarPingPrefab;
        [SerializeField] protected float radarRange = 10f;
        protected readonly HashSet<RadarTarget> registeredTargets = new();
        protected readonly Dictionary<RadarTarget, GameObject> radarPings = new();
        protected void OnEnable()
        {
            GlobalUpdater.Instance.RegisterLateUpdate(UpdateRadar);
        }
        protected void OnDisable()
        {
            GlobalUpdater.Instance.UnregisterLateUpdate(UpdateRadar);
            registeredTargets.Clear();
            foreach (var ping in radarPings.Values) Destroy(ping);
            radarPings.Clear();
        }
        protected virtual void UpdateRadar(float dt)
        {
            foreach (var target in registeredTargets) ProcessTarget(target);
        }
        protected void ProcessTarget(RadarTarget target)
        {
            var distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance > radarRange + target.RadarSignature) return;
            var ping = radarPings[target];
            ping.transform.GetChild(0).localScale = Vector3.one * Mathf.Clamp01(1 - (distance / radarRange));

            ping.transform.localPosition = Vector3.Lerp(Vector3.zero, ((target.transform.position - transform.position) / distance) * radius, Mathf.Clamp01(distance / radarRange));

            var line = ping.GetComponent<LineRenderer>();
            var basePosition = new Vector3(ping.transform.localPosition.x, 0, ping.transform.localPosition.z);
            line.SetPosition(0, ping.transform.TransformVector(basePosition));
            line.SetPosition(1, ping.transform.position);
        }
        public virtual void RegisterTarget(RadarTarget target)
        {
            registeredTargets.Add(target);
            if (!radarPings.ContainsKey(target))
            {
                var ping = Instantiate(radarPingPrefab, center);
                radarPings[target] = ping;
            }
        }
        public virtual void UnregisterTarget(RadarTarget target)
        {
            if (radarPings.TryGetValue(target, out var ping)) Destroy(ping);
            registeredTargets.Remove(target);
        }
    }
}