using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
namespace BudgetAnimancer
{
    public class BudgetAnimancerComponent : MonoBehaviour
    {
        protected PlayableGraph graph;
        protected AnimationLayerMixerPlayable layerMixer;
        protected Animator animator;
        public List<Layer> Layers { get; protected set; } = new();
        protected void Awake()
        {
            animator = transform.root.GetComponentInChildren<Animator>();

            graph = PlayableGraph.Create("BudgetAnimancerComponent");
            graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            // Root layer Mixer
            layerMixer = AnimationLayerMixerPlayable.Create(graph, 1);

            var output = AnimationPlayableOutput.Create(graph, "AnimancerOutput", animator);
            output.SetSourcePlayable(layerMixer);

            EnsureLayer(0);

            graph.Play();
        }

        public void EnsureLayer(int index)
        {
            while (Layers.Count <= index)
            {
                var layer = new Layer(graph);
                Layers.Add(layer);
                int newIndex = Layers.Count - 1;
                graph.Connect(layer.Mixer, 0, layerMixer, newIndex);
                layerMixer.SetInputWeight(newIndex, 1f);
            }
        }

        public AnimationState CreateOrGetState(AnimationClip clip)
        {
            return Layers[0].CreateOrGetState(clip);
        }

        public AnimationState Play(AnimationClip clip, float duration = 0.25f)
        {
            return Layers[0].Play(clip, duration);
        }

        void Update()
        {
            float dt = Time.deltaTime;
            foreach (var layer in Layers)
            {
                layer.Update(dt);
            }
        }
        void OnDestroy()
        {
            graph.Destroy();
        }
    }
}