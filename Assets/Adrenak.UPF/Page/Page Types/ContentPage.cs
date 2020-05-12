using UnityEngine;

namespace Adrenak.UPF {
    public abstract class ContentPage<T> : ContentPage where T : View {
        new public T Content => content as T;
    }

    public class ContentPage : Page {
        [SerializeField] protected View content;
        public View Content => content;
    }
}
