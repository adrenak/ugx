using System;

namespace Adrenak.UGX {
    public abstract class FormView<T> : View<T> where T : ViewModel {
        public ViewEvent onSubmit = new ViewEvent();
        public ViewEvent onCancel = new ViewEvent();

        public void Submit() =>
            onSubmit.Invoke(this);

        public void Cancel() =>
            onCancel.Invoke(this);
    }
}
