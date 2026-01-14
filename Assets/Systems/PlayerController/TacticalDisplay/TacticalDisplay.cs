using System.Collections.Generic;
using System.Linq;
using Timers;
using UnityEngine;
using UnityEngine.UI;
namespace PlayerController
{
    public class TacticalDisplay : MonoBehaviour
    {
        class TacticalObject
        {
            public GameObject Object;
            public RectTransform BoundingBox;
        }
        public List<GameObject> Inputs = new();
        CountdownTimer timer = new CountdownTimer(1);
        HashSet<TacticalObject> tacticalObjects = new();
        public RectTransform TargetPrefab;
        Camera cam;
        public RectTransform reticleParent;
        void Awake()
        {
            cam = Camera.main;
            timer.OnTimerStop += () =>
            {
                ReorderBoundingBoxes();
                timer.Start();
            };
        }
        private void OnDestroy()
        {
            timer.Dispose();
        }
        private void OnEnable()
        {
            foreach (var input in Inputs)
            {
                TacticalObject to = new TacticalObject
                {
                    Object = input,
                    BoundingBox = Instantiate(TargetPrefab, reticleParent.transform)
                };
                to.BoundingBox.GetComponent<Image>().color = input.GetComponent<Renderer>().sharedMaterial.color;
                to.BoundingBox.gameObject.name = input.name;
                tacticalObjects.Add(to);
            }
            timer.Start();
        }
        private void OnDisable()
        {
            timer.Stop();
        }
        private void LateUpdate()
        {
            foreach (var a in tacticalObjects)
            {
                // Convert world position to screen position
                Vector3 screenPos = Camera.main.WorldToScreenPoint(a.Object.transform.position);

                RectTransformUtility.ScreenPointToLocalPointInRectangle(reticleParent, screenPos, null, out var localPos);
                a.BoundingBox.localPosition = localPos;

                float dist = Vector3.Distance(cam.transform.position, a.Object.transform.position);
                var size = Mathf.Clamp(5 / (dist + 0.00001f), 2.5f, 20);
                a.BoundingBox.sizeDelta = new Vector2(20, 20) * size;
            }
        }
        void ReorderBoundingBoxes()
        {
            var list = tacticalObjects.ToList();
            list.Sort((a, b) =>
            {
                float distA = Vector3.Distance(Camera.main.transform.position, a.Object.transform.position);
                float distB = Vector3.Distance(Camera.main.transform.position, b.Object.transform.position);
                return distB.CompareTo(distA);
            });
            for (int i = 0; i < list.Count; i++)
            {
                list[i].BoundingBox.SetSiblingIndex(i);
            }
        }
    }
}
