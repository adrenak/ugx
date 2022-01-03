using System;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    [Serializable]
    public class AskPopupState : State {
        public string header;
        public string body;
        public string positive;
        public string negative;
    }

    public class AskPopup : Popup<AskPopupState, bool> {
#pragma warning disable 0649
        [SerializeField] Text headerDisplay;
        [SerializeField] Text bodyDisplay;
        [SerializeField] Text positiveDisplay;
        [SerializeField] Text negativeDisplay;
#pragma warning restore 0649

        bool confirmed;
        bool denied;

        async protected override UniTask<bool> GetResponse() {
            confirmed = denied = false;
            while (!confirmed && !denied)
                await UniTask.Delay(100);

            return confirmed;
        }

        protected override void OnRefresh() {
            headerDisplay.text = State.header;
            bodyDisplay.text = State.body;
            positiveDisplay.text = State.positive;
            negativeDisplay.text = State.negative;
        }

        public void Confirm() => confirmed = true;

        public void Deny() => denied = true;

        protected override void OnStart() { }
    }
}
