using System;
using UnityWeld.Binding;

namespace Adrenak.UPF {
    [Serializable]
    [Binding]
    public abstract class ListItemView<T> : View<T> where T : ListItemViewModel {
        public void Click() {
            BindingContext.Click();
        }
    }
}
