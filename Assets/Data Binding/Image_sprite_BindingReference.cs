using UnityEngine;
using UnityEngine.UI;

public class Image_sprite_BindingReference : BindingReference<Image> {
    public override void OnPulled(object obj) {
        destination.sprite = (Sprite)obj;
    }
}
