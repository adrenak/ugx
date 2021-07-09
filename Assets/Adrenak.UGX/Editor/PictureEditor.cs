using UnityEditor;
using UnityEditor.UI;

using UnityEngine;

namespace Adrenak.UGX.Editor {
    [CustomEditor(typeof(Picture))]
    public class PictureEditor : ImageEditor {
        bool showEvents;
        bool showStatusEvents;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            Picture image = (Picture)target;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.EnumPopup("Current Visibility", image.CurrentVisibility);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            showStatusEvents = EditorGUILayout.Foldout(showStatusEvents, new GUIContent("Status Objects"), true);
            if (showStatusEvents) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("statusRootObj"), new GUIContent("Root Object"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("loaderObj"), new GUIContent("Loader Object"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("errorObj"), new GUIContent("Error Object"));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            showEvents = EditorGUILayout.Foldout(showEvents, new GUIContent("Events"), true);
            if (showEvents) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onLoadStart"), new GUIContent("On Load Start"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onLoadSuccess"), new GUIContent("On Load Success"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onLoadFailure"), new GUIContent("On Load Failure"));
            }
            EditorGUILayout.EndVertical();

            image.refreshOnStart = EditorGUILayout.Toggle("Refresh On Start", image.refreshOnStart);
            image.updateWhenOffScreen = EditorGUILayout.Toggle("Update When Off Screen", image.updateWhenOffScreen);

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Runtime texture compression doesn't work on mobile devices.", MessageType.Info);
            image.compression = (Texture2DCompression)EditorGUILayout.EnumPopup("Texture Compression", image.compression);
            EditorGUILayout.Space();

            image.path = EditorGUILayout.TextField("Source Path", image.path);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Refresh Picture"))
                image.Refresh();
        }
    }
}