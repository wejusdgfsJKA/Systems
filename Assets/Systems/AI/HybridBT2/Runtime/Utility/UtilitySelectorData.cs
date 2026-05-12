using System.Collections.Generic;
using UnityEngine;

namespace HybridBT2
{
    [CreateAssetMenu(menuName = "HybridBT2/Nodes/UtilitySelector")]
    public class UtilitySelectorData : NodeData
    {
        public List<UtilityWrapperData> Children = new();
        protected override Node GetNodeInternal()
        {
            return new UtilitySelector(Name, onEnter, onExit);
        }
        public override Node GetNodeExternal()
        {
            var node = (UtilitySelector)base.GetNodeExternal();
            foreach (var item in Children) node.AddChild(item.GetNodeExternal());
            return node;
        }
    }
}
