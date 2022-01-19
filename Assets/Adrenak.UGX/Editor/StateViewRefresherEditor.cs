using UnityEngine;
using UnityEditor;

namespace Adrenak.UGX.Editor {
    [CustomEditor(typeof(StateViewRefresher))]
    public class StateViewRefresherEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            var refresher = (StateViewRefresher)target;

            refresher.refreshOnAwake = EditorGUILayout.Toggle("Refresh on Awake()", refresher.refreshOnAwake);
            refresher.refreshOnStart = EditorGUILayout.Toggle("Refresh on Start()", refresher.refreshOnStart);

            if (GUILayout.Button("Refresh Now"))
                refresher.gameObject.SendMessage("RefreshFromState");
        }
    }
}
