using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    public class AlertPopup : Window {
#pragma warning disable 0649
        [SerializeField] Text headerDisplay;
        [SerializeField] Text bodyDisplay;
        [SerializeField] Text ackDisplay;
#pragma warning restore 0649

        async public UniTask Show(string header, string body, string ack) {
            onWindowOpen?.Invoke();

            headerDisplay.text = header;
            bodyDisplay.text = body;
            ackDisplay.text = ack;

            bool responded = false;
            OnAcknowledge = () => responded = true;
            while (!responded)
                await UniTask.Delay(100);

            onWindowClose?.Invoke();
        }

        Action OnAcknowledge;
        public void Acknowledge() {
            OnAcknowledge?.Invoke();
        }
    }
}
