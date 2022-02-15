using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    [Serializable]
    public class AskWithFlagPopupState : State {
        public string header;
        public string body;
        public bool boolFlag;
        public string positive;
        public string negative;
    }

    public class AskWithFlagPopupResponse {
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

        bool confirmed, denied;

        async protected override UniTask<AskWithFlagPopupResponse> GetResponse() {
            confirmed = denied = true;

            while (!confirmed && !denied)
                await UniTask.Delay(100);

            return new AskWithFlagPopupResponse {
                boolFlag = flagDisplay.isOn,
                positive = confirmed
            };            
        }

        protected override void OnInitializeView() { }

        protected override void OnUpdateView() {
            headerDisplay.text = State.header;
            bodyDisplay.text = State.body;
            positiveDisplay.text = State.positive;
            negativeDisplay.text = State.negative;
            flagDisplay.isOn = State.boolFlag;
        }

        public void Confirm() => confirmed = true;

        public void Deny() => denied = true;
    }
}
