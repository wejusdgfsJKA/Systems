using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UtilityAI
{
    [CustomEditor(typeof(UtilityBrain), true)]
    public class BrainEditor : Editor
    {
        void OnEnable()
        {
            RequiresConstantRepaint();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); // Draw the default inspector

            UtilityBrain brain = (UtilityBrain)target;

            if (Application.isPlaying)
            {
                var actions = brain.Actions.OrderByDescending((a) =>
                {
                    return a.Consideration.Evaluate(brain.Context);
                });
                foreach (var action in actions)
                {
                    EditorGUILayout.LabelField("Current Action:", action.name);
                    EditorGUILayout.LabelField("Utility:", action.Consideration.Evaluate(brain.Context).ToString());
                    EditorGUILayout.LabelField("");
                }
            }
        }
    }
}