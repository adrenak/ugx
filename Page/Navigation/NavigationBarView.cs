using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public abstract class NavigationBarView : View {
        [SerializeField] Button backButton;
        [SerializeField] Navigator navigator;

        void Awake() {
            backButton.onClick.AddListener(Back);
        }

        async public void Back() {
            await navigator.PopAsync();
        }
    }
}
