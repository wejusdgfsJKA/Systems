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
                AIActionData chosenAction = brain.CurrentActionIndex != -1 ? brain.Actions[brain.CurrentActionIndex] : null;

                if (chosenAction != null)
                {
                    EditorGUILayout.LabelField("Current Action:", chosenAction.name);
                }
                else
                {
                    EditorGUILayout.LabelField("Current Action:", "None");
                }

            }
        }
    }
}