using System.Collections.Generic;
using UnityEngine;
namespace Tags
{
    public static class TagManager
    {
        static readonly Dictionary<Transform, List<Tags>> transformTagDict = new();
        static readonly Dictionary<Tags, HashSet<Transform>> tagTransformDict = new();
        public static void Register(Transform transform, List<Tags> tags)
        {
            if (tags == null || tags.Count == 0)
            {
                Debug.LogWarning($"Trying to register {transform} with no tags. Ignoring.");
                return;
            }
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
            if (transform == null)
            {
                Debug.LogWarning($"Trying to deregister a null transform. Ignoring.");
                return;
            }
            if (transformTagDict.TryGetValue(transform, out List<Tags> tags))
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
        public static List<Tags> GetTag(Transform transform)
        {
            if (transform == null)
            {
                Debug.LogWarning($"Trying to get tags for a null transform. Ignoring.");
                return new();
            }
            transformTagDict.TryGetValue(transform, out List<Tags> tags);
            return tags;
        }
        /// <summary>
        /// Get all the transforms with a given tag.
        /// </summary>
        /// <param name="tag">The tag we are searching for.</param>
        /// <returns>All transforms with said tag.</returns>
        public static List<Transform> GetByTag(Tags tag)
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
        public static List<Transform> GetByTag(List<Tags> tags)
        {
            if (tags == null || tags.Count == 0)
            {
                Debug.LogWarning($"Trying to get transforms with no tags. Ignoring.");
                return new();
            }
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
        public static void Clear()
        {
            transformTagDict.Clear();
            tagTransformDict.Clear();
        }
    }
}