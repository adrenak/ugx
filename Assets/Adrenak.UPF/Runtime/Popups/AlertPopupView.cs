using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public class AlertPopupView : PopupView {
#pragma warning disable 0649
        [SerializeField] Text headerDisplay;
        [SerializeField] Text bodyDisplay;
        [SerializeField] Text ackDisplay;
#pragma warning restore 0649

        async public Task Show(string header, string body, string ack) {
            onPopupOpen?.Invoke();

            headerDisplay.text = header;
            bodyDisplay.text = body;
            ackDisplay.text = ack;

            bool responded = false;
            OnAcknowledge = () => responded = true;
            while (!responded)
                await Task.Delay(100);

            onPopupClose?.Invoke();
        }

        Action OnAcknowledge;
        public void Acknowledge() {
            OnAcknowledge?.Invoke();
        }
    }
}
