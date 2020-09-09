using System;

namespace Adrenak.UGX {
    [Serializable]
    public abstract class FormViewModel : ViewModel {
        public ViewEvent onSubmit = new ViewEvent();
        public ViewEvent onCancel = new ViewEvent();

        public void Submit() =>
            onSubmit.Invoke(this);

        public void Cancel() =>
            onCancel.Invoke(this);
    }

    public abstract class FormView<T> : View<T> where T : FormViewModel {
        public void Submit() {
            ViewModel.Submit();
        }

        public void Cancel() {
            ViewModel.Cancel();
        }
    }
}
