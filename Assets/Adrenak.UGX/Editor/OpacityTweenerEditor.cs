using UnityEditor;

using UnityEngine;

namespace Adrenak.UGX.Editor {
    [CustomEditor(typeof(OpacityTweener))]
    public class OpacityTweenerEditor : TweenerBaseEditor {
        public override void OnInspectorGUI() {
            GUI.backgroundColor = new Color(.5f, .5f, 1f);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            base.OnInspectorGUI();
            GUILayout.Space(20);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();
        }
    }
}