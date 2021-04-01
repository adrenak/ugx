using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    [System.Serializable]
    public class PopupState : WindowState { }

    [System.Serializable]
    public class PopupResponse { }

    public abstract class Popup<T, K> : Window<T>  where T : PopupState where K : PopupResponse {
        sealed protected override void HandleWindowStateSet() => HandlePopupStateSet();
        
        public abstract UniTask<K> WaitForResponse();

        protected abstract void HandlePopupStateSet();
    }
}
