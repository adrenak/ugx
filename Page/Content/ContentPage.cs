using UnityEngine;
using UnityWeld.Binding;

namespace Adrenak.UPF {
    [Binding]
    public abstract class ContentPage<T> : Page where T : View {
        [SerializeField] T content;
        [Binding]
        public T Content => content;
    }

    [Binding]
    public abstract class ContentPage : Page {
        [SerializeField] View content;
        [Binding]
        public View Content => content;
    }
}
