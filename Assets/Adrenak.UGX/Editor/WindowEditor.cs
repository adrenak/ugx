using UnityEditor;

using UnityEngine;

namespace Adrenak.UGX.Editor {
    [CustomEditor(typeof(Window))]
    public class WindowEditor : UnityEditor.Editor {
        bool showEvents;

        public override void OnInspectorGUI() {
            GUI.backgroundColor = new Color(1f, .5f, .5f);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("status"));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("icon"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("title"));

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("activeTweeners"));
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            showEvents = EditorGUILayout.Foldout(showEvents, new GUIContent("Events"), true);
            if (showEvents) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onWindowStartOpening"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onWindowDoneOpening"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onWindowStartClosing"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onWindowDoneClosing"));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
