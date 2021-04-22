using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Adrenak.UGX {
    [CustomEditor(typeof(Picture))]
    public class PictureEditor : ImageEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            Picture image = (Picture)target;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.EnumPopup("Current Visibility", image.CurrentVisibility);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("onSpriteSet"), new GUIContent("On Sprite Set"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onSpriteRemoved"), new GUIContent("On Sprite Removed"));
            image.refreshOnStart = (bool)EditorGUILayout.Toggle("Refresh On Start", image.refreshOnStart);
            image.compression = (Texture2DCompression)EditorGUILayout.EnumPopup("Texture Compression", image.compression);
            image.source = (Picture.Source)EditorGUILayout.EnumPopup("Source Type", image.source);

            if(image.source != Picture.Source.Asset)
                image.path = EditorGUILayout.TextField("Source Path", image.path);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Refresh Picture"))
                image.Refresh();            
        }
    }
}