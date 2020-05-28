using System;

namespace Adrenak.UPF {
    [Serializable]
    public abstract class FormModel : ViewModel {
        public event EventHandler OnSubmitForm;
        public event EventHandler OnCancelForm;

        public void Submit() {
            OnSubmitForm?.Invoke(this, EventArgs.Empty);
        }

        public void Cancel() {
            OnCancelForm?.Invoke(this, EventArgs.Empty);
        }
    }
}
