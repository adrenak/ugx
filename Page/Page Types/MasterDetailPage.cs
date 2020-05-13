using UnityEngine;
using NaughtyAttributes;

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

        [SerializeField] bool isDetailPageOpenOnStart;
        [ReadOnly] [SerializeField] bool isDetailPageOpen;
        public bool IsDetailPageOpen {
            get => isDetailPageOpen;
            set {
                if (value == IsDetailPageOpen) return;

                isDetailPageOpen = value;
                if (value)
                    Navigator.PushAsync(Detail);
                else
                    Navigator.PopAsync();
            }
        }

        void Start() {
            IsDetailPageOpen = isDetailPageOpenOnStart;
        }
    }
}
