using UnityEngine;

namespace HybridBT2
{
    [CreateAssetMenu(menuName = "HybridBT2/Decorators/Inverter", fileName = "Inverter")]
    public class InverterData: DecoratorData
    {
        protected override Node GetNodeInternal()
        {
            return new Inverter(Name, onEnter, onExit);
        }
    }
}