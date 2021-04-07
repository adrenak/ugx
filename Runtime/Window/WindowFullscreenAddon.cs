using UnityEngine;

namespace Adrenak.UGX {
    [RequireComponent(typeof(Window))]
    public class WindowFullscreenAddon : UGXBehaviour {
        public bool isFullscreen;

        void Start() {
            window.onWindowOpen.AddListener(() => Screen.fullScreen = isFullscreen);
        }
    }
}
