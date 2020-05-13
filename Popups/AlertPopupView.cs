using System;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public class AlertPopup : ViewModel {
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

        protected override void InitializeFromContext() {
            headerDisplay.text = Context.Header;
            bodyDisplay.text = Context.Body;
            ackDisplay.text = Context.Ack;
        }

        public void Dismiss() {
            Context.Dismiss();
        }

        protected override void BindViewToContext() { }
        protected override void OnPropertyChange(string propertyName) { }
    }
}
