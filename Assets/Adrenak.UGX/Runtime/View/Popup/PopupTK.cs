using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    /// <summary>
    /// Base class for popups. Inherit from this class to implement different kinds of popups
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    [RequireComponent(typeof(Window))]
    public abstract class Popup<T, K> : View<T> where T : ViewModel {
        /// <summary>
        /// Called when the popup is shown.
        /// </summary>
        /// <returns>The response object.</returns>
        protected abstract UniTask<K> GetResponse();

        /// <summary>
        /// Displays the popup and returns the response as a task
        /// </summary>
        async public UniTask<K> Show() {
            await UniTask.SwitchToMainThread();
            await Window.OpenWindowAsync();
            var response = await GetResponse();
            await Window.CloseWindowAsync();
            await UniTask.SwitchToMainThread();
            return response;
        }

        /// <summary>
        /// Displays the popup and returns the response as a callback
        /// </summary>
        async public void Show(Action<K> responseCallback) =>
            responseCallback?.Invoke(await Show());

        /// <summary>
        /// Displays the popup with a given model and returns the response as a task
        /// </summary>
        async public UniTask<K> Show(T model) {
            Model = model;
            return await Show();
        }

        /// <summary>
        /// Displays the popup with a given model and returns the response as a callback
        /// </summary>
        async public void Show(T model, Action<K> responseCallback) =>
            responseCallback?.Invoke(await Show(model));

        /// <summary>
        /// Displays the popup with a given model modifier action and returns the response as a task
        /// </summary>
        async public UniTask<K> Show(Action<T> viewModelAccess) {
            viewModelAccess?.Invoke(Model);
            UpdateView();
            return await Show();
        }

        /// <summary>
        /// Displays the popup with a given model modifier action and returns the response as a callback
        /// </summary>
        async public void Show(Action<T> viewModelAccess, Action<K> responseCallback) =>
            responseCallback?.Invoke(await Show(viewModelAccess));
    }
}
