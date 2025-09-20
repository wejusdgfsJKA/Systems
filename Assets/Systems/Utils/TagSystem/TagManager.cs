using System.Collections.Generic;
using UnityEngine;
namespace Tags
{
    public static class TagManager<Tag>
    {
        static Dictionary<Transform, Tag> tagDict = new();
        public static void Register(Transform transform, Tag tag)
        {
            tagDict.Add(transform, tag);
        }
        public static void DeRegister(Transform transform)
        {
            tagDict.Remove(transform);
        }
        public static Tag GetTag(Transform transform)
        {
            tagDict.TryGetValue(transform, out Tag tag);
            return tag;
        }
    }
}