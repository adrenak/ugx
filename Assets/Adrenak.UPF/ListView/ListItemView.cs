using System;
using UnityWeld.Binding;

namespace Adrenak.UPF {
    [Binding]
    public abstract class ListItemView<T> : View<T> where T : ViewModel {
        public event EventHandler OnClick;

        public void Click() {
            OnClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
