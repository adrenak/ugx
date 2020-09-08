using UnityEngine.Events;
using UnityEngine;
using NaughtyAttributes;

namespace Adrenak.UPF {
    public abstract class Dialog : Page {
        //[SerializeField] bool showEvents;
        //[ShowIf("showEvents")] 
        public UnityEvent onPopupOpen;

        //[ShowIf("showEvents")] 
        public UnityEvent onPopupClose;
    }
}
