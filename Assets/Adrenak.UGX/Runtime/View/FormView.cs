using System;

namespace Adrenak.UGX {
    [Serializable]
    public abstract class FormViewModel : ViewModel {
        public ViewEvent submit;
        public ViewEvent cancel;

        public void Submit() =>
            submit.Invoke(this);

        public void Cancel() =>
            cancel.Invoke(this);
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
