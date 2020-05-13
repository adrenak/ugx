namespace Adrenak.UPF {
    public abstract class FormView<T> : View<T> where T : FormViewModel {
        public void Submit() {
            Context.Submit();
        }

        public void Cancel() {
            Context.Cancel();
        }
    }
}
