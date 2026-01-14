using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace PlayerController
{
    public class UIRaycast : MonoBehaviour
    {
        public bool b;
        EventSystem eventSystem;
        GraphicRaycaster raycaster;
        private void Awake()
        {
            raycaster = GetComponentInChildren<GraphicRaycaster>();
            eventSystem = EventSystem.current;
        }
        void Update()
        {
            if (b)
            {
                b = false;
                var pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = new Vector2(Screen.width / 2, Screen.height / 2);
                Debug.Log(pointerEventData.position);
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, results); // Use GraphicRaycaster directly

                //the order of these is the hierarchy order in the reticleParent inverted
                string s = "";
                foreach (RaycastResult result in results)
                {
                    s += result.gameObject.name + ";";
                }
                Debug.Log(s);
            }
        }
    }
}