using UnityEngine;

namespace Adrenak.UGX {
    [RequireComponent(typeof(Window))]
    public class WindowScreenOrientationAddon : UGXBehaviour {
        public ScreenOrientation orientation;
        void Start() {
            window.onWindowDoneOpening.AddListener(() => Screen.orientation = orientation);
        }
    }
}
