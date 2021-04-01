using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    [Serializable]
    public class AskPopupState : PopupState {
        public string header;
        public string body;
        public string positive;
        public string negative;
    }

    public class AskPopupResponse : PopupResponse{
        public bool positive;
    }

    public class AskPopup : Popup<AskPopupState, AskPopupResponse> {
#pragma warning disable 0649
        [SerializeField] Text headerDisplay;
        [SerializeField] Text bodyDisplay;
        [SerializeField] Text positiveDisplay;
        [SerializeField] Text negativeDisplay;
#pragma warning restore 0649

        async public override UniTask<AskPopupResponse> WaitForResponse() {
            // Wait till be get a response
            bool? response = null;
            OnConfirm = () => response = true;
            OnDeny = () => response = false;
            while (response == null)
                await UniTask.Delay(100);
            return new AskPopupResponse { positive = response.Value };
        }

        protected override void HandleViewStateSet() {
            headerDisplay.text = MyViewState.header;
            bodyDisplay.text = MyViewState.body;
            positiveDisplay.text = MyViewState.positive;
            negativeDisplay.text = MyViewState.negative;
        }

        Action OnConfirm;
        public void Confirm() =>
            OnConfirm?.Invoke();

        Action OnDeny;
        public void Deny() => OnDeny?.Invoke();
    }
}
