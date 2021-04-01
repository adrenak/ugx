using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Adrenak.UGX{
    public class WindowScreenOrientationAddon : UGXBehaviour {
        public ScreenOrientation orientation;
        void Start(){
            window.onWindowOpen.AddListener(() => Screen.orientation = orientation);
        }
    }
}
