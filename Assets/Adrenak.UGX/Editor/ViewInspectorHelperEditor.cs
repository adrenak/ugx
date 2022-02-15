using UnityEngine;
using UnityEditor;

namespace Adrenak.UGX.Editor {
    [CustomEditor(typeof(ViewInspectorHelper))]
    public class ViewInspectorHelperEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            var refresher = (ViewInspectorHelper)target;

            if (GUILayout.Button("Update View Now"))
                refresher.gameObject.SendMessage("UpdateView_Internal");
        }
    }
}
