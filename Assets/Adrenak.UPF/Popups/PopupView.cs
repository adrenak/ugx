using UnityEngine.Events;
using UnityEngine;

namespace Adrenak.UPF {
    public abstract class PopupView<T> : View<T> where T : Model {
        [SerializeField] protected UnityEvent onPopupOpen;
        [SerializeField] protected UnityEvent onPopupClose;
    }
}
