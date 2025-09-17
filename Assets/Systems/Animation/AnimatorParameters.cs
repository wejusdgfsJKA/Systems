using UnityEngine;
namespace Animation
{
    public abstract class AnimatorParameter
    {
        public readonly int Hash;
        protected AnimatorParameter(string name) => Hash = Animator.StringToHash(name);
    }

    public sealed class BoolParameter : AnimatorParameter
    {
        public BoolParameter(string name) : base(name) { }
        public void Set(Animator animator, bool value) => animator.SetBool(Hash, value);
        public bool Get(Animator animator) => animator.GetBool(Hash);
    }

    public sealed class IntParameter : AnimatorParameter
    {
        public IntParameter(string name) : base(name) { }
        public void Set(Animator animator, int value) => animator.SetInteger(Hash, value);
        public int Get(Animator animator) => animator.GetInteger(Hash);
    }
    public sealed class FloatParameter : AnimatorParameter
    {
        public FloatParameter(string name) : base(name) { }
        public void Set(Animator animator, float value) => animator.SetFloat(Hash, value);
        public float Get(Animator animator) => animator.GetFloat(Hash);
    }
    public sealed class TriggerParameter : AnimatorParameter
    {
        public TriggerParameter(string name) : base(name) { }
        public void Set(Animator animator) => animator.SetTrigger(Hash);
        public void Reset(Animator animator) => animator.ResetTrigger(Hash);
    }
}