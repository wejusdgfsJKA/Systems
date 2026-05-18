using System.Collections.Generic;
using UnityEngine;

namespace HybridBT2
{
    [CreateAssetMenu(menuName = "HybridBT2/Nodes/UtilitySelector")]
    public class UtilitySelectorData : NodeData
    {
        [Tooltip("The amount of utility that must be gained to switch to another child")]
        public float UtilityDelta = 0;
        public List<UtilityWrapperData> Children = new();
        protected override Node GetNodeInternal()
        {
            return new UtilitySelector(Name, onEnter, onExit, UtilityDelta);
        }
        public override Node GetNodeExternal()
        {
            var node = (UtilitySelector)base.GetNodeExternal();
            foreach (var item in Children) node.AddChild(item.GetNodeExternal());
            return node;
        }
    }
}
