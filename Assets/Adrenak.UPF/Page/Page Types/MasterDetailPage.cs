using UnityEngine;
using NaughtyAttributes;

namespace Adrenak.UPF {
    public abstract class MasterDetailPage<TMasterPage, TDetailPage> : MasterDetailPage where TMasterPage : Page where TDetailPage : Page {
        new public TMasterPage Master => master as TMasterPage;
        new public TDetailPage Detail => detail as TDetailPage;
    }

    public class MasterDetailPage : Page {
#pragma warning disable 0649
        [SerializeField] protected ContentPage master;
        [SerializeField] protected ContentPage detail;
        [SerializeField] bool isDetailPageOpenOnStart;
        [ReadOnly] [SerializeField] bool isDetailPageOpen;
#pragma warning restore 0649

        public ContentPage Master => master;
        public ContentPage Detail => detail;
        public bool IsDetailPageOpen {
            get => isDetailPageOpen;
            set {
                if (value == IsDetailPageOpen) return;

                isDetailPageOpen = value;
                if (value)
                    Navigator.Push(Detail);
                else
                    Navigator.Pop();
            }
        }

        void Start() {
            IsDetailPageOpen = isDetailPageOpenOnStart;
        }
    }
}
