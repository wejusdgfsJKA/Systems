using TMPro;
using UnityEngine;
namespace Sample
{
    public class HPDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;
        public void OnHPChange(float newValue)
        {
            text.text = newValue.ToString();
        }
    }
}