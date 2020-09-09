using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    public class AskPopup : Window {
#pragma warning disable 0649
        [SerializeField] Text headerDisplay;
        [SerializeField] Text bodyDisplay;
        [SerializeField] Text positiveDisplay;
        [SerializeField] Text negativeDisplay;
#pragma warning restore 0649

        async public Task<bool> Show(string header, string body, string positive, string negative) {
            OpenWindow();
            onWindowOpen?.Invoke();

            headerDisplay.text = header;
            bodyDisplay.text = body;
            positiveDisplay.text = positive;
            negativeDisplay.text = negative;

            // Wait till be get a response
            bool? response = null;
            OnConfirm = () => response = true;
            OnDeny = () => response = false;
            while (response == null)
                await Task.Delay(100);

            onWindowClose?.Invoke();
            CloseWindow();
            return response.Value;
        }

        Action OnConfirm;
        public void Confirm() =>
            OnConfirm?.Invoke();

        Action OnDeny;
        public void Deny() =>
            OnDeny?.Invoke();
    }
}
