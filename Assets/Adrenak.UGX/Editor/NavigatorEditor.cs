using UnityEditor;

using UnityEngine;

namespace Adrenak.UGX.Editor {
    [CustomEditor(typeof(Navigator))]
    public class NavigatorEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            var navigator = (Navigator)target;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("WindowPushed"), new GUIContent("On Window Push"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("WindowPopped"), new GUIContent("On Window Pop"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("WindowsOver"), new GUIContent("On WindowS Over"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("popOnEscape"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("sequential"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("windows"));
            
            EditorGUILayout.Space(-20);
            EditorGUI.BeginDisabledGroup(true);
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("router"), new GUIContent("Active Window"));
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}