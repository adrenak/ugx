using UnityEngine;
using UnityEngine.Events;

namespace Adrenak.UPF {
    public class PopupTweener : MonoBehaviour {
#pragma warning disable 0649
        [SerializeField] UnityEvent onFinishOpening;
        [SerializeField] UnityEvent onFinishClosing;
        [SerializeField] PopupView popup;
        [SerializeField] UITweener tweener;
#pragma warning restore 0649

        void Awake() {
            popup.onPopupOpen.AddListener(async () => {
                await tweener.MoveIn();
                tweener.FadeInAndForget();
                onFinishOpening?.Invoke();
            });

            popup.onPopupClose.AddListener(async () => {
                await tweener.FadeOut();
                tweener.MoveOutAndForget();
                onFinishClosing?.Invoke();
            });
        }
    }
}
