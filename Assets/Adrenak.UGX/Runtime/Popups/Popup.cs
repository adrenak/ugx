using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    [System.Serializable]
    public class PopupState : ViewState { }

    [System.Serializable]
    public class PopupResponse { }

    public abstract class Popup<T, K> : Window<T>  where T : PopupState where K : PopupResponse {
        public abstract UniTask<K> WaitForResponse();
        protected override abstract void HandleViewStateSet();
    }
}
