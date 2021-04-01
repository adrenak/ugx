using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    [System.Serializable]
    public class PopupState : WindowState { }

    [System.Serializable]
    public class PopupResponse { }

    public abstract class Popup<T, K> : Window<T> where T : PopupState where K : PopupResponse {
        static GameObject activePopup;

        async public UniTask<K> Display(Action<T> cloneState) {
            await UniTask.WaitWhile(() => activePopup != null);

            var instance = GetClone();
            activePopup = instance.gameObject;
            cloneState?.Invoke(instance.CurrentState);
            
            await instance.OpenWindowAsync();
            var response = await instance.WaitForResponse();
            await instance.CloseWindowAsync();

            activePopup = null;
            Destroy(instance.gameObject);

            return response;

        }

        async public UniTask<K> WaitForResponse() {
            await UniTask.SwitchToMainThread();
            var response = await WaitForResponseImpl();
            await UniTask.SwitchToMainThread();
            return response;
        }

        Popup<T, K> GetClone() {
            var instance = MonoBehaviour.Instantiate(gameObject).GetComponent<Popup<T, K>>();
            instance.transform.SetParent(transform.parent, false);
            return instance;
        }

        protected abstract UniTask<K> WaitForResponseImpl();

        sealed protected override void HandleWindowStateSet() => HandlePopupStateSet();
        protected abstract void HandlePopupStateSet();
    }
}
