using UnityEngine;

namespace Adrenak.UGX {
    public class FullscreenWindowAddon : UGXBehaviour {
        public bool isFullscreen;

        void Start() {
            window.onWindowOpen.AddListener(() => Screen.fullScreen = isFullscreen);
        }
    }
}
