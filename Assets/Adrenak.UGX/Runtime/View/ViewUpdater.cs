using UnityEngine;

namespace Adrenak.UGX {
    public class ViewUpdater : MonoBehaviour {
        public bool updateViewOnAwake;
        public bool updateViewOnStart;

        void Awake() {
            if (updateViewOnAwake)
                UpdateView();
        }

        void Start() {
            if (updateViewOnStart)
                UpdateView();
        }

        public void UpdateView() {
            gameObject.SendMessage("UpdateView");
        }
    }
}
