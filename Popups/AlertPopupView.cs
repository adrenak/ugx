using System;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public class AlertPopup : Model {
        public event EventHandler OnDismiss;

        [SerializeField] string header;
        public string Header {
            get => header;
            set => Set(ref header, value);
        }

        [SerializeField] string body;
        public string Body {
            get => body;
            set => Set(ref body, value);
        }

        [SerializeField] string ack;
        public string Ack {
            get => ack;
            set => Set(ref ack, value);
        }

        public void Dismiss() {
            OnDismiss?.Invoke(this, EventArgs.Empty);
        }
    }

    public class AlertPopupView : View<AlertPopup> {
#pragma warning disable 0649
        [SerializeField] Text headerDisplay;
        [SerializeField] Text bodyDisplay;
        [SerializeField] Text ackDisplay;
#pragma warning restore 0649

        protected override void OnViewInitialize() {
            headerDisplay.text = Model.Header;
            bodyDisplay.text = Model.Body;
            ackDisplay.text = Model.Ack;
        }

        public void Dismiss() {
            Model.Dismiss();
        }

        protected override void OnObserveViewEvents() { }
        protected override void OnViewModelPropertyChanged(string propertyName) { }
    }
}
