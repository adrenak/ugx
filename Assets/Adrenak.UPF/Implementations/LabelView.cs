using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Implementations {
    public class LabelView : View<LabelModel> {
    #pragma warning disable 0649
        [SerializeField] Text labelDisplay;
    #pragma warning restore 0649

        protected override void ListenToView() { }

        protected override void InitializeView() {
            labelDisplay.text = Model.Label;
        }

        protected override void OnModelPropertyChanged(string propertyName) {
            labelDisplay.text = Model.Label;  // we have only one property
        }
    }
}