using BudgetAnimancer;
using UnityEngine;
namespace Sample
{
    public class ChodeTest : MonoBehaviour
    {
        BudgetAnimancerComponent component;
        public Vector2 @params;
        Mixer2DState state;
        public Mixer2DStateData data;
        void Start()
        {
            component = GetComponent<BudgetAnimancerComponent>();
            state = component.Layers[0].PlayMixer2D(data);
        }

        // Update is called once per frame
        void Update()
        {
            state.Parameter = @params;
        }
    }
}