using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// Allows to specify the screen orientation when this window opens
    /// </summary>
    [RequireComponent(typeof(Window))]
    public class WindowScreenOrientationAddon : UGXBehaviour {
        public ScreenOrientation orientation;
        void Start() {
            window.onWindowDoneOpening.AddListener(() => Screen.orientation = orientation);
        }
    }
}
