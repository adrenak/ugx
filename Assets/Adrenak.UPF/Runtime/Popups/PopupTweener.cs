using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

namespace Adrenak.UPF {
    public abstract class PopupTweener : MonoBehaviour {
#pragma warning disable 0649
        [SerializeField] UnityEvent onPopupFinishOpening;
        [SerializeField] UnityEvent onPopupFinishClosing;
        [SerializeField] protected PopupView popup;
        [SerializeField] protected UITweener tweener;
#pragma warning restore 0649

        void Awake() {
            popup.onPopupOpen.AddListener(async () => {
                await OnPopupOpen();
                onPopupFinishOpening?.Invoke();
            });

            popup.onPopupClose.AddListener(async () => {
                await OnPopupClose();
                onPopupFinishClosing?.Invoke();
            });
        }

        protected abstract Task OnPopupOpen();
        protected abstract Task OnPopupClose();
    }
}
