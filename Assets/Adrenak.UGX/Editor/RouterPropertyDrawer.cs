using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using UnityEngine;

namespace Adrenak.UGX.Editor {
    [CustomPropertyDrawer(typeof(Router))]
    public class RouterPropertyDrawer : PropertyDrawer {
        //public override VisualElement CreatePropertyGUI(SerializedProperty property) {
        //    var container = new VisualElement();

        //    var activeWindowField = new PropertyField(property.FindPropertyRelative("activeWindow"));

        //    container.Add(activeWindowField);

        //    return container;
        //}

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            
            var activeWindowProperty = property.FindPropertyRelative("activeWindow");

            EditorGUILayout.PropertyField(activeWindowProperty);

            EditorGUI.EndProperty();
        }
    }
}
