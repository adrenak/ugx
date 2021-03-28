namespace Adrenak.UGX {
    public abstract class Form<T> : View<T> where T : ViewState {
        public ViewEvent onSubmit = new ViewEvent();
        public ViewEvent onCancel = new ViewEvent();

        public void Submit() => onSubmit.Invoke(this);
        public void Cancel() => onCancel.Invoke(this);
    }
}
