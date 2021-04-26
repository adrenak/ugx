using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    [SerializeField]
    public class PopupState : ViewState { }

    [Serializable]
    public class PopupResponse { }

    [RequireComponent(typeof(Window))]
    public abstract class Popup<TState, Response> : StatefulView<TState> where TState : PopupState where Response : PopupResponse {
        /// <summary>
        /// Displays the popup and returns the response as a task
        /// </summary>
        async public UniTask<Response> Display() {
            await UniTask.SwitchToMainThread();
            await window.OpenWindowAsync();
            var response = await GetResponse();
            await window.CloseWindowAsync();
            await UniTask.SwitchToMainThread();
            return response;
        }

        /// <summary>
        /// Displays the popup and returns the response as a callback
        /// </summary>
        async public void Display(Action<Response> responseCallback) =>
            responseCallback?.Invoke(await Display());

        /// <summary>
        /// Displays the popup with a given state and returns the response as a task
        /// </summary>
        async public UniTask<Response> SetStateAndDisplay(TState state) {
            State = state;
            return await Display();
        }

        /// <summary>
        /// Displays the popup with a given state and returns the response as a callback
        /// </summary>
        async public void SetStateAndDisplay(TState state, Action<Response> responseCallback) =>
            responseCallback?.Invoke(await SetStateAndDisplay(state));

        /// <summary>
        /// Displays the popup with a given state modifier and returns the response as a task
        /// </summary>
        async public UniTask<Response> ModifyStateAndDisplay(Action<TState> stateModifier) {
            stateModifier?.Invoke(State);
            return await Display();
        }

        /// <summary>
        /// Displays the popup with a given state modifier and returns the response as a callback
        /// </summary>
        async public void ModifyStateAndDisplay(Action<TState> stateModifier, Action<Response> responseCallback) =>
            responseCallback?.Invoke(await ModifyStateAndDisplay(stateModifier));

        protected override void HandleStateSet() => HandlePopupStateSet();

        protected abstract void HandlePopupStateSet();

        protected abstract UniTask<Response> GetResponse();
    }
}
