using System;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public class ConfirmationPopup : ViewModel{
        public event EventHandler OnConfirm;
        public event EventHandler OnDeny;

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

        [SerializeField] string positive;
        public string Positive {
            get => positive;
            set => Set(ref positive, value);
        }

        [SerializeField] string negative;
        public string Negative {
            get => negative;
            set => Set(ref negative, value);
        }

        public void Confirm() {
            OnConfirm?.Invoke(this, EventArgs.Empty);
        }

        public void Deny(){
            OnDeny?.Invoke(this, EventArgs.Empty);
        }
    }

    public class ConfirmationPopupView : View<ConfirmationPopup> {
#pragma warning disable 0649
        [SerializeField] Text headerDisplay;
        [SerializeField] Text bodyDisplay;
        [SerializeField] Text positiveDisplay;
        [SerializeField] Text negativeDisplay;
#pragma warning restore 0649

        protected override void InitializeFromContext() {
            headerDisplay.text = Context.Header;
            bodyDisplay.text = Context.Body;
            positiveDisplay.text = Context.Positive;
            negativeDisplay.text = Context.Negative;
        }

        public void Confirm() {
            Context.Confirm();
        }

        public void Deny() {
            Context.Deny();
        }

        protected override void BindViewToContext() { }
        protected override void OnPropertyChange(string propertyName) { }
    }
}
