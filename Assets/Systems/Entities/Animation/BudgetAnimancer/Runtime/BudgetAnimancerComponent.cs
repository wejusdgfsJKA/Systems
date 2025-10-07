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
        /// <summary>
        /// Animation layers.
        /// </summary>
        public List<Layer> Layers { get; } = new();
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

        /// <summary>
        /// Make sure a layer of given index exists. Will create layers until Layers.count is equal to index.
        /// </summary>
        /// <param name="index">The index that we need to ensure a layer at.</param>
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

        /// <summary>
        /// Returns the coresponding animation state on layer 0. Calls EnsureLayer(0).
        /// </summary>
        /// <param name="clip">The animation clip.</param>
        /// <returns>The created animation state.</returns>
        public AnimState CreateOrGetState(AnimationClip clip)
        {
            if (clip == null)
            {
                Debug.LogError($"{this} cannot create animation state from null clip!");
                return null;
            }
            EnsureLayer(0);
            return Layers[0].CreateOrGetAnimationState(clip);
        }

        /// <summary>
        /// Play a certain animation on layer 0. Calls EnsureLayer(0).
        /// </summary>
        /// <param name="clip">The animation clip.</param>
        /// <param name="blendDuration">How long should blending take.</param>
        /// <returns>The corresponding animation state.</returns>
        public AnimState Play(AnimationClip clip, float blendDuration = 0.25f)
        {
            if (clip == null)
            {
                Debug.LogError($"{this} cannot create animation state from null clip!");
                return null;
            }
            EnsureLayer(0);
            return Layers[0].Play(clip, blendDuration);
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