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

            // TODO: want to get rid of these from Window.cs
            // In the meantime I'll stop drawing them.
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("icon"));
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("title"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dontTweenToSameStatus"));

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("activeTweeners"));
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            showEvents = EditorGUILayout.Foldout(showEvents, new GUIContent("Events"), true);
            if (showEvents) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WindowStartedOpening"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WindowDoneOpening"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WindowStartedClosing"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WindowDoneClosing"));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
