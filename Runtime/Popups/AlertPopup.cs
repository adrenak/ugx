using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    [Serializable]
    public class AlertPopupState : PopupState {
        public string header;
        public string description;
        public string ack;
    }

    public class AlertPopup : Popup<AlertPopupState, PopupResponse> {
        [SerializeField] Text headerDisplay = null;
        [SerializeField] Text bodyDisplay = null;
        [SerializeField] Text ackDisplay = null;

        Action OnAcknowledge;

        async public override UniTask<PopupResponse> WaitForResponse() {
            bool responded = false;
            OnAcknowledge = () => responded = true;
            while (!responded)
                await UniTask.Delay(100);
            return new PopupResponse();
        }

        protected override void HandleViewStateSet() {
            headerDisplay.text = MyViewState.header;
            bodyDisplay.text = MyViewState.description;
            ackDisplay.text = MyViewState.ack;
        }

        public void Acknowledge() {
            OnAcknowledge?.Invoke();
        }
    }
}
