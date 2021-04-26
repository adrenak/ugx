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

        async protected override UniTask<PopupResponse> GetResponse() {
            bool responded = false;
            OnAcknowledge = () => responded = true;
            while (!responded)
                await UniTask.Delay(100);
            return new PopupResponse();
        }

        protected override void HandlePopupStateSet() {
            headerDisplay.text = State.header;
            bodyDisplay.text = State.description;
            ackDisplay.text = State.ack;
        }

        public void Acknowledge() {
            OnAcknowledge?.Invoke();
        }
    }
}
