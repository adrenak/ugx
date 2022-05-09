using UnityEditor;
using UnityEditor.UI;

using UnityEngine;

namespace Adrenak.UGX.Editor {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RectTransformPositionTweener))]
    public class RectTransformPositionTweenerEditor : TweenerBaseEditor {
        public override void OnInspectorGUI() {
            GUI.backgroundColor = new Color(.5f, .5f, 1f);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            base.OnInspectorGUI();
            GUILayout.Space(20);
            GUI.backgroundColor = Color.white;
            RectTransformPositionTweener tweener = (RectTransformPositionTweener)target;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Stored Positions", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("inPosition"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("outPosition"));
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Edge Config", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enterEdge"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("exitEdge"));
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Set As In"))
                if(EditorUtility.DisplayDialog("Confirmation", "Set current position as IN position?", "Yes", "No"))
                    tweener.CaptureInPosition();
            if (GUILayout.Button("Set As Out"))
                if (EditorUtility.DisplayDialog("Confirmation", "Set current position as OUT position?", "Yes", "No"))
                    tweener.CaptureOutPosition();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
}