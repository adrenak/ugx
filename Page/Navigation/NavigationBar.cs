using UnityEngine;

namespace Adrenak.UPF {
    public class NavigationBar : View {
        [SerializeField] Navigator navigator;

        public void Back() {
            navigator.PopAsync();
        }
    }
}
