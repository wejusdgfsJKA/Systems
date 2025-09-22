using System.Collections.Generic;
using UnityEngine;
namespace Tags
{
    public static class TagManager<Tag>
    {
        static Dictionary<Transform, List<Tag>> transformTagDict = new();
        static Dictionary<Tag, HashSet<Transform>> tagTransformDict = new();
        public static void Register(Transform transform, List<Tag> tags)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (!tagTransformDict.TryGetValue(tags[i], out HashSet<Transform> transforms))
                {
                    transforms = new();
                    tagTransformDict.Add(tags[i], transforms);
                }
                transforms.Add(transform);
            }
            transformTagDict.Add(transform, tags);
        }
        public static void DeRegister(Transform transform)
        {
            if (transformTagDict.TryGetValue(transform, out List<Tag> tags))
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    var transforms = tagTransformDict[tags[i]];
                    transforms.Remove(transform);
                    if (transforms.Count == 0)
                    {
                        tagTransformDict.Remove(tags[i]);
                    }
                }
                transformTagDict.Remove(transform);
            }
        }
        public static List<Tag> GetTag(Transform transform)
        {
            transformTagDict.TryGetValue(transform, out List<Tag> tags);
            return tags;
        }
        /// <summary>
        /// Get all the transforms with a given tag.
        /// </summary>
        /// <param name="tag">The tag we are searching for.</param>
        /// <returns>All transforms with said tag.</returns>
        public static List<Transform> GetByTag(Tag tag)
        {
            if (tagTransformDict.TryGetValue(tag, out HashSet<Transform> transforms))
            {
                return new List<Transform>(transforms);
            }
            return new();
        }
        /// <summary>
        /// Get all transforms with any of the given tags.
        /// </summary>
        /// <param name="tags">All the tags we are searching for.</param>
        /// <returns>All transforms with any of the tags.</returns>
        public static List<Transform> GetByTag(List<Tag> tags)
        {
            List<Transform> trs = new();
            for (int i = 0; i < tags.Count; i++)
            {
                if (tagTransformDict.TryGetValue(tags[i], out HashSet<Transform> transforms))
                {
                    trs.AddRange(transforms);
                }
            }
            return trs;
        }
    }
}