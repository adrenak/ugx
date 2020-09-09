using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    [ExecuteAlways]
    public class ImageAspectRatioFitter : AspectRatioFitter {
        Image image;
        Image Image => image ?? (image = GetComponent<Image>());

        Texture2D tex;

        new void Update() {
            if (Image == null || Image.sprite == null) return;

            var t = Image.sprite.texture;
            if (t == tex)
                return;
            tex = t;
            aspectRatio = (float) t.width / t.height;
        }
    }
}
