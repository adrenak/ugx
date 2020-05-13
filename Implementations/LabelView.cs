using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Implementations {
    public class LabelView : View<LabelVM> {
    #pragma warning disable 0649
        [SerializeField] Text labelDisplay;
    #pragma warning restore 0649

        protected override void BindViewToContext() { }

        protected override void OnSetContext() {
            labelDisplay.text = Context.Label;
        }

        protected override void OnPropertyChange(string propertyName) {
            labelDisplay.text = Context.Label;  // we have only one property
        }
    }
}