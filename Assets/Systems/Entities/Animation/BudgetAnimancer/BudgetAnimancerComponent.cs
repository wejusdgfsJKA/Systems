using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
namespace BudgetAnimancer
{
    public class BudgetAnimancerComponent : MonoBehaviour
    {
        private PlayableGraph graph;
        private AnimationLayerMixerPlayable layerMixer;
        private Animator animator;

        // Represents a playable state
        public class State
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

        // Layer container
        private class Layer
        {
            public AnimationMixerPlayable mixer;
            public List<State> states = new();
            public Dictionary<AnimationClip, State> stateCache = new();
        }

        private List<Layer> layers = new();

        void Awake()
        {
            animator = GetComponent<Animator>();

            graph = PlayableGraph.Create("BudgetAnimancerComponent");
            graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            // Root layer mixer
            layerMixer = AnimationLayerMixerPlayable.Create(graph, 1);

            var output = AnimationPlayableOutput.Create(graph, "AnimancerOutput", animator);
            output.SetSourcePlayable(layerMixer);

            // Base layer
            layers.Add(new Layer
            {
                mixer = AnimationMixerPlayable.Create(graph, 0)
            });

            graph.Connect(layers[0].mixer, 0, layerMixer, 0);
            layerMixer.SetInputWeight(0, 1f);

            graph.Play();
        }

        private void EnsureLayer(int index)
        {
            while (layers.Count <= index)
            {
                var layer = new Layer
                {
                    mixer = AnimationMixerPlayable.Create(graph, 0)
                };
                layers.Add(layer);

                int newIndex = layers.Count - 1;
                graph.Connect(layer.mixer, 0, layerMixer, newIndex);
                layerMixer.SetInputWeight(newIndex, 1f);
            }
        }

        public State Play(AnimationClip clip, int layerIndex = 0)
        {
            EnsureLayer(layerIndex);
            var layer = layers[layerIndex];

            if (layer.stateCache.TryGetValue(clip, out var state))
            {
                // Reactivate existing state
                state.fadeSpeed = 0f;
                state.weight = 1f;

                int inputIndex = layer.states.IndexOf(state);
                layer.mixer.SetInputWeight(inputIndex, 1f);
                return state;
            }

            // Create new state
            var playable = AnimationClipPlayable.Create(graph, clip);
            int inputIndex2 = layer.mixer.GetInputCount();
            layer.mixer.SetInputCount(inputIndex2 + 1);
            graph.Connect(playable, 0, layer.mixer, inputIndex2);

            state = new State { playable = playable, weight = 1f, fadeSpeed = 0f };
            layer.states.Add(state);
            layer.mixer.SetInputWeight(inputIndex2, 1f);

            layer.stateCache[clip] = state;
            return state;
        }

        public void CrossFade(AnimationClip clip, float duration, int layerIndex = 0)
        {
            EnsureLayer(layerIndex);
            var layer = layers[layerIndex];

            var state = Play(clip, layerIndex);

            // Fade out all other states
            foreach (var s in layer.states)
            {
                if (s == state) continue;
                s.fadeSpeed = -1f / duration;
            }

            // Fade in new state
            state.weight = 0f;
            state.fadeSpeed = 1f / duration;
        }

        void Update()
        {
            foreach (var layer in layers)
            {
                for (int i = 0; i < layer.states.Count; i++)
                {
                    var s = layer.states[i];
                    if (!s.IsFading) continue;

                    s.weight = Mathf.Clamp01(s.weight + s.fadeSpeed * Time.deltaTime);
                    layer.mixer.SetInputWeight(i, s.weight);

                    if (s.weight <= 0f)
                    {
                        s.fadeSpeed = 0f; // stop fading out but keep state in cache
                        layer.mixer.SetInputWeight(i, 0f);
                    }
                    else if (s.weight >= 1f)
                    {
                        s.fadeSpeed = 0f; // done fading in
                        layer.mixer.SetInputWeight(i, 1f);
                    }
                }
            }
        }

        void OnDestroy()
        {
            graph.Destroy();
        }
    }
}