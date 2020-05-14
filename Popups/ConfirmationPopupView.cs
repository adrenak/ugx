using System;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public class ConfirmationPopup : Model{
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

        protected override void InitializeView() {
            headerDisplay.text = Model.Header;
            bodyDisplay.text = Model.Body;
            positiveDisplay.text = Model.Positive;
            negativeDisplay.text = Model.Negative;
        }

        public void Confirm() {
            Model.Confirm();
        }

        public void Deny() {
            Model.Deny();
        }

        protected override void ListenToView() { }
        protected override void OnModelPropertyChanged(string propertyName) { }
    }
}
