using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples{
    public class MovieView : View<MovieModel> {
#pragma warning disable 0649
        [SerializeField] Text nameDisplay;
        [SerializeField] Text ratingDisplay;
#pragma warning restore 0649

        protected override void InitializeView() {
            nameDisplay.text = Model.Name;
            ratingDisplay.text = Model.Rating.ToString("0.00");
        }

        protected override void ListenToView() { }
        protected override void OnModelPropertyChanged(string propertyName) { }
    }
}
