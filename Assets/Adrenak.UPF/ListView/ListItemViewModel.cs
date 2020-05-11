using System;

namespace Adrenak.UPF{
    public class ListItemViewModel : ViewModel {
        public event EventHandler OnClick;

        public void Click() {
            OnClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
