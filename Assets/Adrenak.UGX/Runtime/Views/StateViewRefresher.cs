using UnityEngine;

namespace Adrenak.UGX {
    public class StateViewRefresher : MonoBehaviour {
        public bool refreshOnAwake;
        public bool refreshOnStart;

        void Awake() {
            if (refreshOnAwake)
                gameObject.SendMessage("RefreshFromState");
        }

        void Start() {
            if (refreshOnStart)
                gameObject.SendMessage("RefreshFromState");
        }
    }
}
