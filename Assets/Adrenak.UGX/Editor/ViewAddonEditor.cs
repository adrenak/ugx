using UnityEngine;
using UnityEditor;

namespace Adrenak.UGX.Editor {
    [CustomEditor(typeof(ViewAddon))]
    public class ViewAddonEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            var refresher = (ViewAddon)target;

            if (GUILayout.Button("Update View"))
                refresher.gameObject.SendMessage("UpdateView_Internal");
            if (GUILayout.Button("Reset View"))
                refresher.gameObject.SendMessage("ResetView");
        }
    }
}
