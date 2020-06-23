namespace Adrenak.UPF {
    public abstract class FormView<T> : View<T> where T : FormModel {
        public void Submit() {
            ViewModel.Submit();
        }

        public void Cancel() {
            ViewModel.Cancel();
        }
    }
}
