using System;
using UnityWeld.Binding;

namespace Adrenak.UPF {
    [Binding]
    public class ListItemView<T> : View<T> where T : ViewModel {
        public event Action<ListItemView<T>> Clicked;

        public void OnClick() {
            Clicked?.Invoke(this);
        }
    }
}
