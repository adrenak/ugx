namespace Adrenak.UPF {
    public abstract class FormView<T> : View<T> where T : Form {
        public void Submit() {
            Context.Submit();
        }

        public void Cancel() {
            Context.Cancel();
        }
    }
}
