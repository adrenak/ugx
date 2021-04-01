using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    public class PopupState : ViewState { }

    public class PopupResult { }

    public abstract class Popup<T, K> : Window<T>  where T : PopupState where K : PopupResult {
        public abstract UniTask<K> WaitForResponse();
        protected override abstract void HandleViewStateSet();
    }
}
