using UnityEngine;
using UnityWeld.Binding;

namespace Adrenak.UPF {
    [Binding]
    public abstract class MasterDetailPage<TMasterPage, TDetailPage> : Page where TMasterPage : Page where TDetailPage : Page {
        [SerializeField] TMasterPage master;
        [Binding]
        public TMasterPage Master => master;

        [SerializeField] TDetailPage detail;
        [Binding]
        public TDetailPage Detail => detail;

        [SerializeField] bool isDetailPresented;
        [Binding]
        public bool IsDetailPresented {
            get => isDetailPresented;
            set {
                Set(ref isDetailPresented, value);

                if (value) {
                    master.OnDisappearing();
                    detail.OnAppearing();
                }
                else {
                    detail.OnDisappearing();
                    master.OnAppearing();
                }
            }
        }
    }
}
