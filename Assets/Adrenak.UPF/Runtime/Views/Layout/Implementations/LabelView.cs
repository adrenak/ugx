using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Implementations {
    public class LabelView : View<LabelModel> {
    #pragma warning disable 0649
        [SerializeField] Text labelDisplay;
    #pragma warning restore 0649

        protected override void ObserveView() { }

        protected override void Refresh() {
            labelDisplay.text = Model.Label;
        }

        protected override void ObserveModel(string propertyName) {
            labelDisplay.text = Model.Label;  // we have only one property
        }
    }
}