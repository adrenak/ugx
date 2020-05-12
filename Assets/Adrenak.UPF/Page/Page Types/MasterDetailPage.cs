using UnityEngine;

namespace Adrenak.UPF {
    public abstract class MasterDetailPage<TMasterPage, TDetailPage> : MasterDetailPage where TMasterPage : Page where TDetailPage : Page {
        new public TMasterPage Master => master as TMasterPage;
        new public TDetailPage Detail => detail as TDetailPage;
    }

    public class MasterDetailPage : Page {
        [SerializeField] protected ContentPage master;
        public ContentPage Master => master;

        [SerializeField] protected ContentPage detail;
        public ContentPage Detail => detail;

        bool isDetailPageOpen;
        public bool IsDetailPageOpen {
            get => isDetailPageOpen;
            set {
                isDetailPageOpen = value;
                if (value)
                    Navigator.PushAsync(Detail);
                else
                    Navigator.PopAsync();
            }
        }
    }
}
