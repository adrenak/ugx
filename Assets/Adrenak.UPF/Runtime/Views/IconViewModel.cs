using System;

namespace Adrenak.UPF {
    [Serializable]
    public class DIconViewModel : ViewModel {
        public event EventHandler OnClick;
        public void Click() => OnClick?.Invoke(this, null);

        public string text;
        public string imageURL;
    }
}