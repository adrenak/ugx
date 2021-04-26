using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace Adrenak.UGX {
    [CustomEditor(typeof(Window))]
    public class WindowEditor : Editor {
        bool showEvents;

        public override void OnInspectorGUI() {
            Window window = (Window)target;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.EnumPopup("Status", window.Status);
            EditorGUI.EndDisabledGroup();

            window.autoPopOnBack = EditorGUILayout.Toggle("Auto-Pop on back", window.autoPopOnBack);

            showEvents = EditorGUILayout.Foldout(showEvents, new GUIContent("Events"));
            if (showEvents) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onWindowStartOpening"), new GUIContent("On Window Start Opening"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onWindowDoneOpening"), new GUIContent("On Window Done Opening"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onWindowStartClosing"), new GUIContent("On Window Start Closing"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onWindowDoneClosing"), new GUIContent("On Window Done Closing"));
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
