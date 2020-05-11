using UnityWeld.Binding;

namespace Adrenak.UPF {
    [Binding]
    public abstract class ListItemView<T> : View<T> where T : ListItemViewModel {
        public void Click() {
            BindingContext.Click();
        }
    }
}
