using UnityWeld.Binding;

namespace Adrenak.UPF.Examples {
    [Binding]
    public class SignUpFormView : View<SignUpFormViewModel> {
        public void Submit() {
            BindingContext.Submit();
        }

        public void Cancel() {
            BindingContext.Cancel();
        }
    }
}
