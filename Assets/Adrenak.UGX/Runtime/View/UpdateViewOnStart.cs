using UnityEngine;

namespace Adrenak.UGX {
    public class UpdateViewOnStart : MonoBehaviour {
        void Start() {
            gameObject.SendMessage("UpdateView");
        }
    }
}
