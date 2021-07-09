using UnityEditor;

using UnityEngine;

namespace Adrenak.UGX.Editor {
    [CustomEditor(typeof(Navigator))]
    public class NavigatorEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            var navigator = (Navigator)target;
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onPush"), new GUIContent("On Window Push"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onPop"), new GUIContent("On Window Pop"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("ID"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("canPopAll"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("initialWindow"));

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("current"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("history"));
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}