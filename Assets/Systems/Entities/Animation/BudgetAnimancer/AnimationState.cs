using UnityEngine.Animations;
using UnityEngine.Playables;
namespace BudgetAnimancer
{
    public class AnimationState
    {
        public AnimationClipPlayable playable;
        public float weight;
        public float fadeSpeed;

        public bool IsFading => fadeSpeed != 0;

        public float Time
        {
            get => (float)playable.GetTime();
            set => playable.SetTime(value);
        }

        public float Speed
        {
            get => (float)playable.GetSpeed();
            set => playable.SetSpeed(value);
        }
    }
}
