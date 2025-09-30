using BudgetAnimancer;
using UnityEngine;
namespace Sample
{
    public class ChodeTest : MonoBehaviour
    {
        BudgetAnimancerComponent component;
        public Vector2 @params;
        void Start()
        {
            component = GetComponent<BudgetAnimancerComponent>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}