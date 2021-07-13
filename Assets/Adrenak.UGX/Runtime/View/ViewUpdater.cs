using UnityEngine;

namespace Adrenak.UGX {
    public class ViewUpdater : MonoBehaviour {
        public bool updateViewOnAwake;
        public bool updateViewOnStart;

        void Awake() {
            if (updateViewOnAwake)
                UpdateTheView();
        }

        void Start() {
            if (updateViewOnStart)
                UpdateTheView();
        }

        public void UpdateTheView() {
            gameObject.SendMessage("UpdateView");
        }
    }
}
