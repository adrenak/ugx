using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    /// <summary>
    /// Base class for states of popup implementations
    /// </summary>
    [SerializeField]
    public class PopupState : ViewState { }

    /// <summary>
    /// Base class for response of popup implementation
    /// </summary>
    [Serializable]
    public class PopupResponse { }

    /// <summary>
    /// Base class for popups. Inherit from this class to implement different kinds of popups
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <typeparam name="Response"></typeparam>
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
        /// Displays the popup with a given state modifier action and returns the response as a task
        /// </summary>
        async public UniTask<Response> ModifyStateAndDisplay(Action<TState> stateModifier) {
            stateModifier?.Invoke(State);
            UpdateView();
            return await Display();
        }

        /// <summary>
        /// Displays the popup with a given state modifier action and returns the response as a callback
        /// </summary>
        async public void ModifyStateAndDisplay(Action<TState> stateModifier, Action<Response> responseCallback) =>
            responseCallback?.Invoke(await ModifyStateAndDisplay(stateModifier));

        protected abstract UniTask<Response> GetResponse();

        [Obsolete(".HandleStateSet instead")]
        protected virtual void HandlePopupStateSet() => OnStateSet();
    }
}
