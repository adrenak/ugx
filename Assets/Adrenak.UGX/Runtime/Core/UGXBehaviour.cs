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

        public static T InstantiateUGXBehaviourResource<T>(string path) where T : UGXBehaviour {
            var resource = Resources.Load<T>(path);
            return Instantiate(resource);
        }

        public View view => GetComponent<View>();
        public Window window => GetComponent<Window>();
        public TransitionerBase[] transitioners => GetComponents<TransitionerBase>();
    }
}
