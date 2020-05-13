using UnityEngine;

namespace Adrenak.UPF {
    public abstract class ContentPage<TViewModel> : ContentPage where TViewModel : View {
        new public TViewModel Content => content as TViewModel;
    }

    public class ContentPage : Page {
        [SerializeField] protected View content;
        public View Content => content;
    }
}
