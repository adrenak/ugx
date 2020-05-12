using System;
using UnityWeld.Binding;

namespace Adrenak.UPF {
    [Serializable]
    [Binding]
    public class ListItemViewModel : ViewModel {
        public event EventHandler OnClick;

        public void Click() {
            OnClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
