using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
namespace BudgetAnimancer
{
    public class Mixer2DState : MixerState
    {
        protected float xParameter;
        public float XParameter
        {
            get
            {
                return xParameter;
            }
            set
            {
                if (xParameter != value)
                {
                    xParameter = value;
                    ParameterValueChanged();
                }
            }
        }
        protected float yParameter;
        public float YParameter
        {
            get
            {
                return yParameter;
            }
            set
            {
                if (yParameter != value)
                {
                    yParameter = value;
                    ParameterValueChanged();
                }
            }
        }
        protected List<Vector2> thresholds = new();
        public Mixer2DState(PlayableGraph graph, int index, List<(Vector2, AnimationClip, float)> motionFields, float defaultXValue = 0, float defaultYValue = 0) : base(graph, index)
        {
            var Mixer = (AnimationMixerPlayable)Playable;
            xParameter = defaultXValue;
            yParameter = defaultYValue;
            for (int i = 0; i < motionFields.Count; i++)
            {
                var playable = AnimationClipPlayable.Create(graph, motionFields[i].Item2);
                playable.SetSpeed(motionFields[i].Item3);
                Mixer.SetInputWeight(i, 0);
                graph.Connect(playable, 0, Mixer, i);
                thresholds.Add(motionFields[i].Item1);
            }
            ParameterValueChanged();
        }

        protected override void ParameterValueChanged()
        {
            throw new System.NotImplementedException();
        }
    }
}