using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    /// <summary>
    /// An extended AspectRatioFitter that automaticaly
    /// sets up the ratio to make sure the image on the gameobject
    /// isn't stretched by adapting to its ratio.
    /// </summary>
    [ExecuteAlways]
    public class ImageAspectRatioFitter : AspectRatioFitter {
        Image image;
        Image Image => image == null ? image = GetComponent<Image>() : image;

        Texture2D tex;

        new void Update() {
            base.Update();
            if (Image == null || Image.sprite == null) {
                aspectRatio = 1;
                return;
            }

            var t = Image.sprite.texture;
            if (t == tex)
                return;
            tex = t;
            aspectRatio = (float)t.width / t.height;
        }
    }
}