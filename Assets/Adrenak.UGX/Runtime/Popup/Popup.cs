using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    public class PopupState : ViewState { }

    [Serializable]
    public class PopupResponse { }

    [RequireComponent(typeof(Window))]
    public abstract class Popup<T, K> : StatefulView<T> where T : PopupState where K : PopupResponse {
        static View activePopup;

        async public UniTask<K> WaitForResponse() {
            await UniTask.SwitchToMainThread();
            var response = await WaitForResponseImpl();
            await UniTask.SwitchToMainThread();
            return response;
        }

        async public void WaitForResponse(Action<K> responseCallback) =>
            responseCallback?.Invoke(await WaitForResponse());

        async public UniTask<K> Display() {
            await UniTask.WaitWhile(() => activePopup != null);

            activePopup = this;
            await activePopup.window.OpenWindowAsync();
            var response = await WaitForResponse();
            await window.CloseWindowAsync();
            activePopup = null;

            return response;
        }

        async public void Display(Action<K> responseCallback) =>
            responseCallback?.Invoke(await Display());

        async public UniTask<K> Display(T state) {
            State = state;
            return await Display();
        }

        async public void Display(T state, Action<K> responseCallback) =>
            responseCallback?.Invoke(await Display(state));

        async public UniTask<K> Display(Action<T> stateModifier) {
            stateModifier?.Invoke(State);
            return await Display();
        }

        async public void Display(Action<T> stateModifier, Action<K> responseCallback) =>
            responseCallback?.Invoke(await Display(stateModifier));

        protected override void HandleStateSet() => HandlePopupStateSet();

        protected abstract void HandlePopupStateSet();

        protected abstract UniTask<K> WaitForResponseImpl();
    }
}
