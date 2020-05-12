using System;
using UnityWeld.Binding;

namespace Adrenak.UPF.Examples {
    [Serializable]
    [Binding]
    public class MovieListItemView : View<MovieCell> {
        public void Click() {
            BindingContext.Click();
        }
    }
}
