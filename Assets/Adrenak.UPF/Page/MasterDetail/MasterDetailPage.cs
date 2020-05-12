using UnityEngine;


namespace Adrenak.UPF {
    
    public abstract class MasterDetailPage<TMasterPage, TDetailPage> : Page where TMasterPage : Page where TDetailPage : Page {
        [SerializeField] TMasterPage master;
        
        public TMasterPage Master => master;

        [SerializeField] TDetailPage detail;
        
        public TDetailPage Detail => detail;

        [SerializeField] bool isDetailPresented;
        
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
