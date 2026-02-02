using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Assertions;
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
        protected List<float> startLayerWeights = new(), targetLayerWeights = new(),
            currentLayerWeights = new(), fadeElapsed = new(), fadeTimes = new();
        protected void Awake()
        {
            animator = transform.root.GetComponentInChildren<Animator>();

            graph = PlayableGraph.Create("BudgetAnimancerComponent");
            graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            // Root layerIndex Mixer
            layerMixer = AnimationLayerMixerPlayable.Create(graph, 1);

            var output = AnimationPlayableOutput.Create(graph, "AnimancerOutput", animator);
            output.SetSourcePlayable(layerMixer);

            EnsureLayer(0);

            graph.Play();
        }

        /// <summary>
        /// Make sure a layerIndex of given index exists. Will create layers until Layers.count is equal to index.
        /// </summary>
        /// <param name="index">The index that we need to ensure a layerIndex at.</param>
        /// <param name="mask">The mask the layerIndex will use. Optional.</param>
        public void EnsureLayer(int index, AvatarMask mask = null, bool additive = false)
        {
            Assert.IsTrue(index >= 0);
            while (Layers.Count <= index)
            {
                var layer = new Layer(graph);
                Layers.Add(layer);
                layerMixer.SetInputCount(Layers.Count);
                int newIndex = Layers.Count - 1;
                graph.Connect(layer.Mixer, 0, layerMixer, newIndex);
                var initialWeight = newIndex == 0 ? 1 : 0;
                layerMixer.SetInputWeight(newIndex, initialWeight);
                targetLayerWeights.Add(1);
                fadeTimes.Add(0);
                startLayerWeights.Add(initialWeight);
                currentLayerWeights.Add(initialWeight);
                fadeElapsed.Add(0);
            }
            if (mask != null) layerMixer.SetLayerMaskFromAvatarMask((uint)index, mask);
            layerMixer.SetLayerAdditive((uint)index, additive);
            Assert.AreEqual(layerMixer.GetInputCount(), Layers.Count);
            Assert.AreEqual(targetLayerWeights.Count, Layers.Count);
            Assert.AreEqual(targetLayerWeights.Count, fadeTimes.Count);
            Assert.AreEqual(targetLayerWeights.Count, startLayerWeights.Count);
            Assert.AreEqual(targetLayerWeights.Count, fadeElapsed.Count);
            Assert.AreEqual(currentLayerWeights.Count, fadeElapsed.Count);
        }

        /// <summary>
        /// Returns the coresponding animation state on layerIndex 0. Calls EnsureLayer(0).
        /// </summary>
        /// <param name="clip">The animation clip.</param>
        /// <returns>The created animation state.</returns>
        public AnimState CreateOrGetAnimationState(AnimationClip clip)
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
        /// Play a certain animation on layerIndex 0. Calls EnsureLayer(0).
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
        public void SetLayerWeight(int layerIndex, float newWeight, float fadeTime = 0.1f)
        {
            EnsureLayer(layerIndex);
            if (fadeTime > 0f)
            {
                startLayerWeights[layerIndex] = currentLayerWeights[layerIndex];
                targetLayerWeights[layerIndex] = newWeight;
                fadeElapsed[layerIndex] = 0;
                fadeTimes[layerIndex] = Mathf.Max(0.0001f, fadeTime);
            }
            else
            {
                startLayerWeights[layerIndex] = currentLayerWeights[layerIndex] = targetLayerWeights[layerIndex] = newWeight;
                layerMixer.SetInputWeight(layerIndex, newWeight);
            }
        }
        protected void Update()
        {
            float dt = Time.deltaTime;
            foreach (var layer in Layers)
            {
                layer.Update(dt);
            }
        }

        protected void LateUpdate()
        {
            var dt = Time.deltaTime;
            for (int i = 0; i < Layers.Count; i++)
            {
                var currentWeight = currentLayerWeights[i];
                if (Mathf.Approximately(currentWeight, targetLayerWeights[i])) continue;
                fadeElapsed[i] += dt;

                float t = fadeElapsed[i] / fadeTimes[i];
                t = Mathf.Clamp01(t);

                float weight = Mathf.Lerp(
                    startLayerWeights[i],
                    targetLayerWeights[i],
                    t
                );
                currentLayerWeights[i] = weight;
                layerMixer.SetInputWeight(i, weight);
            }
        }

        void OnDestroy()
        {
            graph.Destroy();
        }
    }
}