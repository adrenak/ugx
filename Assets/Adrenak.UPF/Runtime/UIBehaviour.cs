using UnityEngine;

namespace Adrenak.UPF {
    public class UIBehaviour : MonoBehaviour {
        public View view => GetComponent<View>();
        public Page page => GetComponent<Page>();
        public Transitioner transitioner => GetComponent<Transitioner>();
    }
}
