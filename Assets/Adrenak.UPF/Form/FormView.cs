using UnityWeld.Binding;

namespace Adrenak.UPF {
    [Binding]
    public abstract class FormView<T> : View<T> where T : FormViewModel {
        public void Submit() {
            BindingContext.Submit();
        }

        public void Cancel() {
            BindingContext.Cancel();
        }
    }
}
