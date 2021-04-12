using UnityEngine;

namespace Adrenak.UGX {
    [RequireComponent(typeof(Window))]
    public class WindowScreenOrientationAddon : UGXBehaviour {
        public ScreenOrientation orientation;
        void Start() {
            window.onWindowOpen.AddListener(() => Screen.orientation = orientation);
        }
    }
}
