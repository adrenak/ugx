namespace Adrenak.UGX {
    public class PopupResponse<K> {
        public bool HasData { get; private set; }
        public K Data { get; private set; }

        public PopupResponse() {
            HasData = false;
            Data = default;
        }

        public PopupResponse(K data) {
            HasData = true;
            Data = data;
        }
    }
}