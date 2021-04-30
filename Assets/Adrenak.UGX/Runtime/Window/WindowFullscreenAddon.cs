using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// Allows to specify the full screen mode should when this window opens
    /// </summary>
    [RequireComponent(typeof(Window))]
    public class WindowFullscreenAddon : UGXBehaviour {
        public bool isFullscreen;

        void Start() {
            window.onWindowDoneOpening.AddListener(() => Screen.fullScreen = isFullscreen);
        }
    }
}
