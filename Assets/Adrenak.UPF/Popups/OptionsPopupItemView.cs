using System;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public class OptionsPopupItem : ViewModel {
        public event EventHandler OnClick;

        [SerializeField] string label;
        public string Label {
            get => label;
            set => Set(ref label, value);
        }

        public void Click() {
            OnClick?.Invoke(this, EventArgs.Empty);
        }
    }

    public class OptionsPopupItemView : View<OptionsPopupItem> {
#pragma warning disable 0649
        [SerializeField] Text labelDisplay;
        
#pragma warning restore 0649

        protected override void InitializeFromContext() {
            labelDisplay.text = Context.Label;
        }

        public void Click() {
            Context.Click();
        }

        protected override void BindViewToContext() { }
        protected override void OnPropertyChange(string propertyName) { }
    }
}
