using System;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples {
    [Serializable]  
    public class MovieView : View<Movie> {
        [SerializeField] Text nameDisplay;
        [SerializeField] Text ratingDisplay;

        protected override void InitializeFromContext() {
            nameDisplay.text = Context.Name;
            ratingDisplay.text = Context.Rating.ToString("0.00");
        }

        protected override void BindViewToContext() {
            // Not a 2 way binding
        }

        protected override void OnPropertyChange(string propertyName) {
            switch(propertyName){
                case nameof(Context.Name):
                    nameDisplay.text = Context.Name;
                    break;
                case nameof(Context.Rating):
                    ratingDisplay.text = Context.Rating.ToString("0.00");
                    break;
            }
        }

        public void Click() {
            Context.Click();
        }
    }
}
