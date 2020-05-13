using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples{
    public class MovieView : View<MovieVM> {
        [SerializeField] Text nameDisplay;
        [SerializeField] Text ratingDisplay;

        protected override void BindViewToContext() { }

        protected override void OnSetContext() {
            nameDisplay.text = Context.Name;
            ratingDisplay.text = Context.Rating.ToString("0.00");
        }

        protected override void OnPropertyChange(string propertyName) { }
    }
}
