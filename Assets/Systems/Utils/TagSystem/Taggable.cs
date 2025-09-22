using System.Collections.Generic;
using UnityEngine;
namespace Tags
{
    public class Taggable<T> : MonoBehaviour
    {
        [SerializeField] protected List<T> currentTags;
        protected void OnEnable()
        {
            TagManager<T>.Register(transform, currentTags);
        }
        protected void OnDisable()
        {
            TagManager<T>.DeRegister(transform);
        }
    }
}