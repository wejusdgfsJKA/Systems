using System.Collections.Generic;
using UnityEngine;
namespace Tags
{
    public class Taggable : MonoBehaviour
    {
        [SerializeField] protected List<Tags> currentTags;
        protected void OnEnable()
        {
            TagManager.Register(transform, currentTags);
        }
        protected void OnDisable()
        {
            TagManager.DeRegister(transform);
        }
    }
}