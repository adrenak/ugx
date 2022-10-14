using UnityEngine;

namespace Adrenak.UGX {
    public class ViewAddon : UGXBehaviour {
        [SerializeField] bool updateOnStart = true;
        public bool UpdateOnStart {
            get => updateOnStart;
            set => updateOnStart = value;
        }

        void Start() {
            if (UpdateOnStart)
                SendMessage("UpdateView_Internal");
        }
    }
}
