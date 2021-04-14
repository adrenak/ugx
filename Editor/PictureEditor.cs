﻿using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Adrenak.UGX {
    [CustomEditor(typeof(Picture))]
    public class PictureEditor : ImageEditor {
        public override void OnInspectorGUI() {
            Picture image = (Picture)target;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onSpriteSet"), new GUIContent("On Sprite Set"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onSpriteRemoved"), new GUIContent("On Sprite Removed"));
            image.compression = (Texture2DCompression)EditorGUILayout.EnumPopup("Texture Compression", image.compression);
            image.source = (Picture.Source)EditorGUILayout.EnumPopup("Source Type", image.source);
            image.path = EditorGUILayout.TextField("Source Path", image.path);
            image.currentVisibility = (Visibility)EditorGUILayout.EnumPopup("Current Visibility", image.CurrentVisibility);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Refresh"))
                image.Refresh();

            base.OnInspectorGUI();
        }
    }
}