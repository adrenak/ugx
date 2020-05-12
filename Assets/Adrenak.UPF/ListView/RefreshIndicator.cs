namespace Adrenak.UPF {
    public abstract class RefreshIndicator : View {
        public abstract void SetValue(float value);
        public abstract void SetRefreshing(bool state);
    }
}
