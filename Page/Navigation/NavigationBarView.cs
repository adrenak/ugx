using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public abstract class NavigationBarView : View {
#pragma warning disable 0649
        [SerializeField] Button backButton;
        [SerializeField] Navigator navigator;
#pragma warning restore 0649

        void Awake() {
            backButton.onClick.AddListener(Back);
        }

        public void Back() {
            navigator.PopAsync();
        }
    }
}
