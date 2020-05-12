using UnityEngine;


namespace Adrenak.UPF {
    
    public abstract class ContentPage<T> : Page where T : View {
        [SerializeField] T content;
        
        public T Content => content;
    }

    
    public abstract class ContentPage : Page {
        [SerializeField] View content;
        
        public View Content => content;
    }
}
