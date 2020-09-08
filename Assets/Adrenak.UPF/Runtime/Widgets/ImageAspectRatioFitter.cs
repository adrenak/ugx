using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    [ExecuteAlways]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(AspectRatioFitter))]
    public class ImageAspectRatioFitter : MonoBehaviour {
        AspectRatioFitter fitter;
        AspectRatioFitter Fitter => fitter ?? (fitter = GetComponent<AspectRatioFitter>());

        Image image;
        Image Image => image ?? (image = GetComponent<Image>());

        Texture2D tex;

        void Update() {
            if (Image == null || Image.sprite == null) return;

            var t = Image.sprite.texture;
            if (t == tex)
                return;
            tex = t;
            Fitter.aspectRatio = (float) t.width / t.height;
        }
    }
}
