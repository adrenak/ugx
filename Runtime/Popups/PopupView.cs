using UnityEngine.Events;
using UnityEngine;
using NaughtyAttributes;

namespace Adrenak.UPF {
    public abstract class PopupView : BindableBehaviour {
        public bool IsOpen { get; protected set; }

        [SerializeField] bool showEvents;
        [ShowIf("showEvents")] public UnityEvent onPopupOpen;
        [ShowIf("showEvents")] public UnityEvent onPopupClose;
    }
}
