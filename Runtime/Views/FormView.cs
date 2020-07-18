namespace Adrenak.UPF {
    public abstract class FormView<T> : View<T> where T : FormViewModel {
        public void Submit() {
            ViewModel.Submit();
        }

        public void Cancel() {
            ViewModel.Cancel();
        }
    }
}
