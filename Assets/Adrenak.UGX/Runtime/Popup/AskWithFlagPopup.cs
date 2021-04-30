using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    [Serializable]
    public class AskWithFlagPopupState : PopupState {
        public string header;
        public string body;
        public bool boolFlag;
        public string positive;
        public string negative;
    }

    public class AskWithFlagPopupResponse : PopupResponse{
        public bool boolFlag;
        public bool positive;
    }

    public class AskWithFlagPopup : Popup<AskWithFlagPopupState, AskWithFlagPopupResponse> {
#pragma warning disable 0649
        [SerializeField] Text headerDisplay;
        [SerializeField] Text bodyDisplay;
        [SerializeField] Text positiveDisplay;
        [SerializeField] Text negativeDisplay;
        [SerializeField] Toggle flagDisplay;
#pragma warning restore 0649

        async protected override UniTask<AskWithFlagPopupResponse> GetResponse() {
            bool? response = null;
            OnConfirm = () => response = true;
            OnDeny = () => response = false;
            while (response == null)
                await UniTask.Delay(100);
            return new AskWithFlagPopupResponse { 
                positive = response.Value,
                boolFlag = flagDisplay.isOn
            };
        }

        protected override void OnStateSet() {
            headerDisplay.text = State.header;
            bodyDisplay.text = State.body;
            positiveDisplay.text = State.positive;
            negativeDisplay.text = State.negative;
            flagDisplay.isOn = State.boolFlag;
        }

        Action OnConfirm;
        public void Confirm() => OnConfirm?.Invoke();

        Action OnDeny;
        public void Deny() => OnDeny?.Invoke();

        protected override void OnStart() { }
    }
}
