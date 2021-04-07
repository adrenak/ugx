using UnityEngine;

namespace Adrenak.UGX {
    [RequireComponent(typeof(RectTransform))]
    public class UGXBehaviour : MonoBehaviour {
        RectTransform rt;
        public RectTransform RT {
            get {
                if (rt == null)
                    rt = GetComponent<RectTransform>();
                return rt;
            }
        }

        public View view => GetComponent<View>();
        public Window window => GetComponent<Window>();
        public TransitionerBase[] transitioners => GetComponents<TransitionerBase>();
    }
}
