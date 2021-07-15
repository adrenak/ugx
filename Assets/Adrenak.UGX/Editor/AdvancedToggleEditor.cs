using UnityEditor;
using UnityEditor.UI;

namespace Adrenak.UGX.Editor {
    [CustomEditor(typeof(AdvancedToggle))]
    public class AdvancedToggleEditor : ToggleEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("label"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("textOn"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("textOff"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("objOn"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("objOff"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
