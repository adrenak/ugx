using UnityEditor;
using UnityEditor.UI;

namespace Adrenak.UPF {
    [CustomEditor(typeof(DynamicImage))]
    public class MyImageEditor : ImageEditor {
        public override void OnInspectorGUI() {
            DynamicImage image = (DynamicImage)target;
            image.compression = (DynamicImage.Compression)EditorGUILayout.EnumPopup("Texture Compression", image.compression);
            image.source = (DynamicImage.Source)EditorGUILayout.EnumPopup("Source Type", image.source);
            image.path = EditorGUILayout.TextField("Source Path", image.path);
            image.loadOnStart = EditorGUILayout.Toggle("Refresh On Start", image.loadOnStart);

            base.OnInspectorGUI();
        }
    }
}