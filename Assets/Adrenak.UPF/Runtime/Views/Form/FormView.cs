namespace Adrenak.UPF {
    public abstract class FormView<T> : View<T> where T : FormModel {
        public void Submit() {
            Model.Submit();
        }

        public void Cancel() {
            Model.Cancel();
        }
    }
}
