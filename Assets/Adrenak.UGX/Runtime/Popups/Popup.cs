using UnityEngine.Events;

namespace Adrenak.UGX {
    public abstract class Popup : Window {
        public UnityEvent onPopupOpen;
        public UnityEvent onPopupClose;
    }
}
