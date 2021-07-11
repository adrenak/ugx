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
        async public UniTask<PopupResponse<K>> Show() {
            await UniTask.SwitchToMainThread();
            await Window.OpenWindowAsync();

            var response = await GetResponse();
            await Window.CloseWindowAsync();
            await UniTask.SwitchToMainThread();
            return new PopupResponse<K>(response);
        }

        /// <summary>
        /// Displays the popup and returns the response as a callback
        /// </summary>
        async public void Show(Action<bool, K> responseCallback) {
            var response = await Show();
            responseCallback?.Invoke(response.HasData, response.Data);
        }

        /// <summary>
        /// Displays the popup with a given model and returns the response as a task
        /// </summary>
        async public UniTask<PopupResponse<K>> SetModelAndShow(T model) {
            if (Window.Status == WindowStatus.Closed) {
                Model = model;
                return await Show();
            }
            return new PopupResponse<K>();
        }

        /// <summary>
        /// Displays the popup with a given model and returns the response as a callback
        /// </summary>
        async public void SetModelAndShow(T model, Action<bool, K> responseCallback) {
            if (Window.Status == WindowStatus.Closed) {
                var response = await SetModelAndShow(model);
                responseCallback?.Invoke(response.HasData, response.Data);
            }
            responseCallback?.Invoke(false, default);
        }

        /// <summary>
        /// Displays the popup with a given model modifier action and returns the response as a task
        /// </summary>
        async public UniTask<PopupResponse<K>> ModifyModelAndShow(Action<T> viewModelAccess) {
            if (Window.Status == WindowStatus.Closed) {
                ModifyViewModel(viewModelAccess);
                return await Show();
            }
            return new PopupResponse<K>();
        }

        /// <summary>
        /// Displays the popup with a given model modifier action and returns the response as a callback
        /// </summary>
        async public void ModifyModelAndShow(Action<T> modifier, Action<bool, K> responseCallback) {
            if (Window.Status == WindowStatus.Closed) {
                var response = await ModifyModelAndShow(modifier);
                responseCallback?.Invoke(response.HasData, response.Data);
            }
            responseCallback?.Invoke(false, default);
        }
    }
}
