using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

namespace Adrenak.UPF {
    public abstract class PopupTweener : MonoBehaviour {
#pragma warning disable 0649
        [SerializeField] UnityEvent onFinishOpening;
        [SerializeField] UnityEvent onFinishClosing;
        [SerializeField] protected PopupView popup;
        [SerializeField] protected UITweener tweener;
#pragma warning restore 0649

        void Awake() {
            popup.onPopupOpen.AddListener(async () => {
                await OnPopupOpen();
                onFinishOpening?.Invoke();
            });

            popup.onPopupClose.AddListener(async () => {
                await OnPopupClose();
                onFinishClosing?.Invoke();
            });
        }

        protected abstract Task OnPopupOpen();
        protected abstract Task OnPopupClose();
    }
}
