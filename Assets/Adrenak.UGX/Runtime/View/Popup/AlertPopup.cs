using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    [Serializable]
    public class AlertPopupState : ViewModel {
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

        protected override void OnViewModelUpdate() {
            headerDisplay.text = Model.heading;
            bodyDisplay.text = Model.body;
            ackDisplay.text = Model.acknowledgment;
        }

        bool responded;
        public void Acknowledge() {
            responded = true;
        }
    }
}
