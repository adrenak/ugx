using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    /// <summary>
    /// Base class for popups. Inherit from this class to 
    /// implement different kinds of popups
    /// </summary>
    /// <typeparam name="T">
    /// <see cref="State"/> for the popup display
    /// </typeparam>
    /// <typeparam name="K">The response captured by the popup</typeparam>
    [RequireComponent(typeof(Window))]
    public abstract class Popup<T, K> : StateView<T> where T : State {
        /// <summary>
        /// Wait for user response and send it back as a <see cref="K"/> object
        /// </summary>
        /// <returns>The response object.</returns>
        protected abstract UniTask<K> GetResponse();

        /// <summary>
        /// Displays the popup and returns the response as a task
        /// </summary>
        async public UniTask<K> Show() {
            await UniTask.SwitchToMainThread();

            if (Window.IsClosedOrClosing)
                await Window.OpenWindowAsync();

            var response = await GetResponse();
            await Window.CloseWindowAsync();
            await UniTask.SwitchToMainThread();
            return response;
        }

        /// <summary>
        /// Displays the popup and returns the response as a callback
        /// </summary>
        async public void Show(Action<K> responseCallback) {
            var response = await Show();
            responseCallback?.Invoke(response);
        }

        /// <summary>
        /// Sets state and displays the popup. Returns response in a task
        /// </summary>
        async public UniTask<K> SetStateAndShow(T state) {
            Refresh(state);
            return await Show();
        }

        /// <summary>
        /// Sets state and displays the popup. Returns response as a callback
        /// </summary>
        async public void SetStateAndShow(T state, Action<K> response) =>
            response?.Invoke(await SetStateAndShow(state));

        /// <summary>
        /// Gives access to the state for modification and displays the popup. 
        /// Returns response as a task
        /// </summary>
        async public UniTask<K> ModifyStateAndShow(Action<T> access) {
            Refresh(access);
            return await Show();
        }

        /// <summary>
        /// Gives access to the state for modification and displays the popup.
        /// Returns response as a callback
        /// </summary>
        async public void ModifyStateAndShow(Action<T> access, Action<K> response) {
            response?.Invoke(await ModifyStateAndShow(access));
        }
    }
}
