namespace Adrenak.UPF {
    public abstract class ListRefreshIndicator : View {
        public abstract void SetValue(float value);
        public abstract void SetRefreshing(bool state);
    }
}
