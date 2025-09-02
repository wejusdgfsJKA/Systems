using TMPro;
using UnityEngine;

namespace BT
{
    public class BTDebug : MonoBehaviour
    {
        [SerializeField] BehaviourTree LinkedBT;
        [SerializeField] TextMeshProUGUI LinkedDebugText;

        // Start is called before the first frame update
        void Start()
        {
            LinkedDebugText.text = "";
        }

        // Update is called once per frame
        void Update()
        {
            if (LinkedBT != null)
            {
                LinkedDebugText.text = LinkedBT.GetDebugText();
            }
        }
    }
}