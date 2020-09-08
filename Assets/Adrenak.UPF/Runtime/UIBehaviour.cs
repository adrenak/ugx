using UnityEngine;

namespace Adrenak.UPF {
    public class UIBehaviour : MonoBehaviour {
        public View view => GetComponent<View>();
        public Window page => GetComponent<Window>();
        public Transitioner transitioner => GetComponent<Transitioner>();
    }
}
