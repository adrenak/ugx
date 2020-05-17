using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples{
    public class MovieView : View<MovieModel> {
#pragma warning disable 0649
        [SerializeField] Text nameDisplay;
        [SerializeField] Text ratingDisplay;
#pragma warning restore 0649

        protected override void OnSetViewModel() {
            nameDisplay.text = Model.Name;
            ratingDisplay.text = Model.Rating.ToString("0.00");
        }

        protected override void OnObserveView() { }
        protected override void OnViewModelPropertyChanged(string propertyName) { }
    }
}
