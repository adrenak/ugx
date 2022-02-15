using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    [System.Serializable]
    public class NavigatorIconTitleViewModel : State {
        public Sprite sprite;
        public string text;
    }

    /// <summary>
    /// Used to display an icon and title based on the window opened. This
    /// can be used to create title bars where the text and icon changes
    /// depending on the latest <see cref="Window"/> opened by a 
    /// <see cref="Navigator"/>
    /// 
    /// If required, the text and icon displayed can also be changed manually
    /// as this is a <see cref="View{TState}"/> implementation.
    /// </summary>
    public class NavigatorIconTitleView : View<NavigatorIconTitleViewModel> {
        public bool autoUpdateOnNavigatorPush = true;

        [SerializeField] Navigator navigator;
        [SerializeField] Image icon;
        [SerializeField] Text title;

        protected override void OnInitializeView() {
            navigator.WindowPushed.AddListener(window => {
                if (!autoUpdateOnNavigatorPush) return;

                ModifyState(state => {
                    state.sprite = window.icon;
                    state.text = window.title;
                });
            });
        }

        protected override void OnUpdateView() {
            if (icon != null)
                icon.sprite = State.sprite;

            if (title != null)
                title.text = State.text;
        }
    }
}
