using System;

namespace Adrenak.UPF {
    [Serializable]
    public abstract class Form : ViewModel {
        public event EventHandler OnSubmit;
        public event EventHandler OnCancel;

        public void Submit() {
            OnSubmit?.Invoke(this, EventArgs.Empty);
        }

        public void Cancel() {
            OnCancel?.Invoke(this, EventArgs.Empty);
        }
    }
}
