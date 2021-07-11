using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    [System.Serializable]
    public class NavigatorIconTitleViewModel : ViewModel {
        public Sprite sprite;
        public string text;
    }

    public class NavigatorIconTitleView : View<NavigatorIconTitleViewModel> {
        [SerializeField] Navigator navigator;
        [SerializeField] Image icon;
        [SerializeField] Text title;

        void Awake() {
            navigator.onOpen.AddListener(window => {
                if (title != null)
                    title.text = window.title;

                if (icon != null && window.icon != null)
                    icon.sprite = window.icon;
            });
        }

        protected override void OnViewModelUpdate() {
            icon.sprite = Model.sprite;
            title.text = Model.text;
        }
    }
}
