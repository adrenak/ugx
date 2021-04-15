using UnityEngine;

namespace Adrenak.UGX {
    [RequireComponent(typeof(Window))]
    public class WindowFullscreenAddon : UGXBehaviour {
        public bool isFullscreen;

        void Start() {
            window.onWindowDoneOpening.AddListener(() => Screen.fullScreen = isFullscreen);
        }
    }
}
