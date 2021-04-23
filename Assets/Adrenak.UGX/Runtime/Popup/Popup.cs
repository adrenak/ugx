using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    public class PopupState : ViewState { }

    [Serializable]
    public class PopupResponse { }

    [RequireComponent(typeof(Window))]
    public abstract class Popup<T, K> : StatefulView<T> where T : PopupState where K : PopupResponse {
        static GameObject activePopup;

        [Obsolete("It is recommended that you use Popup.Display instead as it manages popup instancing too.")]
        async public UniTask<K> WaitForResponse() {
            await UniTask.SwitchToMainThread();
            var response = await WaitForResponseImpl();
            await UniTask.SwitchToMainThread();
            return response;
        }

        [Obsolete("It is recommended that you use Popup.Display instead as it manages popup instancing too.")]
        public Popup<T, K> GetClone() {
            var instance = MonoBehaviour.Instantiate(gameObject).GetComponent<Popup<T, K>>();
            instance.transform.SetParent(transform.parent, false);
            return instance;
        }

        public static void Display(string path, Action<T> cloneState, Action<K> resultCallback){
            var instance = InstantiateUGXBehaviourResource<Popup<T, K>>(path);
            instance.Display(cloneState, resultCallback);
        }

        async public void Display(Action<T> cloneState, Action<K> resultCallback) {
            var result = await Display(cloneState);
            resultCallback?.Invoke(result);
        }

        async public static UniTask<K> Display(string path, Action<T> cloneState){
            var instance = InstantiateUGXBehaviourResource<Popup<T, K>>(path);
            return await instance.Display(cloneState);
        }

        async public UniTask<K> Display(Action<T> cloneState) {
            await UniTask.WaitWhile(() => activePopup != null);

#pragma warning disable 0618
            var instance = GetClone();
#pragma warning restore 0618
            activePopup = instance.gameObject;
            cloneState?.Invoke(instance.State);

            await (instance as Window).OpenWindowAsync();
#pragma warning disable 0618
            var response = await instance.WaitForResponse();
#pragma warning restore 0618
            await window.CloseWindowAsync();

            activePopup = null;
            Destroy(instance.gameObject);

            return response;
        }

        protected override void HandleStateSet() => HandlePopupStateSet();

        protected abstract void HandlePopupStateSet();

        protected abstract UniTask<K> WaitForResponseImpl();
    }
}
