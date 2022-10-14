using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    [Serializable]
    public class AlertPopupState : State {
        public string heading;
        public string body;
        public string acknowledgment;
    }

    public class AlertPopup : Popup<AlertPopupState, UniTask> {
        [SerializeField] Text headerDisplay = null;
        [SerializeField] Text bodyDisplay = null;
        [SerializeField] Text ackDisplay = null;

        async protected override UniTask<UniTask> GetResponse() {
            responded = false;
            while (!responded)
                await UniTask.Delay(100);
            return UniTask.CompletedTask;
        }

        protected override void OnViewStateChange() {
            headerDisplay.text = State.heading;
            bodyDisplay.text = State.body;
            ackDisplay.text = State.acknowledgment;
        }

        bool responded;
        public void Acknowledge() {
            responded = true;
        }
    }
}
