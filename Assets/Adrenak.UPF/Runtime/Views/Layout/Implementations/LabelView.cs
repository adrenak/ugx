using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Implementations {
    public class LabelView : View<LabelModel> {
    #pragma warning disable 0649
        [SerializeField] Text labelDisplay;
    #pragma warning restore 0649

        protected override void OnObserveView() { }

        protected override void OnSetViewModel() {
            labelDisplay.text = Model.Label;
        }

        protected override void OnViewModelPropertyChanged(string propertyName) {
            labelDisplay.text = Model.Label;  // we have only one property
        }
    }
}