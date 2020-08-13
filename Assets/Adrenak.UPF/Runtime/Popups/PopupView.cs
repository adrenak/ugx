using UnityEngine.Events;
using UnityEngine;
using NaughtyAttributes;

namespace Adrenak.UPF {
    public abstract class PopupView : BindableBehaviour {
        [ReadOnly] [SerializeField] bool isOpen;
        public bool IsOpen { 
            get => isOpen;
            protected set => isOpen = value; 
        }

        [SerializeField] bool showEvents;
        [ShowIf("showEvents")] public UnityEvent onPopupOpen;
        [ShowIf("showEvents")] public UnityEvent onPopupClose;
    }
}
