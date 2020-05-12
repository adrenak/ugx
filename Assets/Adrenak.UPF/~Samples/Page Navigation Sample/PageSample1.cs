using UnityEngine;

namespace Adrenak.UPF.Examples{
    public class PageSample1 : MonoBehaviour {
        public Navigator navigation;

        public ContentPage page2;

        [ContextMenu("Navigate")]
        public void Navigate(){
            navigation.PushAsync(page2);
        }
    }
}
