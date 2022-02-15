using UnityEditor;
using UnityEditor.UI;

using UnityEngine;

namespace Adrenak.UGX.Editor {
    [CustomEditor(typeof(TweenerBase))]
    public class TweenerBaseEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            TweenerBase tweener = (TweenerBase)target;

            GUI.backgroundColor = new Color(.7f, .7f, .7f);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            tweener.progress = EditorGUILayout.Slider("Progress", tweener.progress, 0, 1);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Tweening Args", EditorStyles.boldLabel);
            var useSameArgsForInAndOut = serializedObject.FindProperty("useSameStyleForInAndOut");
            EditorGUILayout.PropertyField(useSameArgsForInAndOut);

            if (useSameArgsForInAndOut.boolValue)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("commonStyle"));
            else {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("inStyle"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("outStyle"));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Tween In"))
                tweener.TweenIn();
            if (GUILayout.Button("Tween Out"))
                tweener.TweenOut();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
}