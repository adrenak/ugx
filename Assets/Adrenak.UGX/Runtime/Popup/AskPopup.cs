using System;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    [Serializable]
    public class AskPopupModel : ViewModel {
        public string header;
        public string body;
        public string positive;
        public string negative;
    }

    public class AskPopupResponse {
        public bool positive;
    }

    public class AskPopup : Popup<AskPopupModel, AskPopupResponse> {
#pragma warning disable 0649
        [SerializeField] Text headerDisplay;
        [SerializeField] Text bodyDisplay;
        [SerializeField] Text positiveDisplay;
        [SerializeField] Text negativeDisplay;
#pragma warning restore 0649

        bool confirmed;
        bool denied;

        async protected override UniTask<AskPopupResponse> GetResponse() {
            confirmed = denied = false;
            while (!confirmed && !denied)
                await UniTask.Delay(100);

            return new AskPopupResponse() {
                positive = confirmed
            };
        }

        protected override void OnViewModelUpdate() {
            headerDisplay.text = Model.header;
            bodyDisplay.text = Model.body;
            positiveDisplay.text = Model.positive;
            negativeDisplay.text = Model.negative;
        }

        public void Confirm() => confirmed = true;

        public void Deny() => denied = true;
    }
}
