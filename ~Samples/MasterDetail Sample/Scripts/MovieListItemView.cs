using System;


namespace Adrenak.UPF.Examples {
    [Serializable]
    
    public class MovieListItemView : View<MovieCell> {
        public void Click() {
            Context.Click();
        }

        protected override void InitializeFromContext() {
            throw new NotImplementedException();
        }

        protected override void BindViewToContext() {
            throw new NotImplementedException();
        }

        protected override void OnPropertyChange(string propertyName) {
            throw new NotImplementedException();
        }
    }
}
