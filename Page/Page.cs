using UnityEngine;

namespace Adrenak.UPF {
    public class Page : BindableBehaviour {
        [SerializeField] string title;        
        public string Title {
            get => title;
            set => Set(ref title, value);
        }

        [SerializeField] Navigator navigator;        
        public Navigator Navigator => navigator;

        void Start() {
            InitializePage();
        }

        protected virtual void InitializePage() { }
        public virtual void OnAppearing() { }
        public virtual void OnDisappearing() { }
        public virtual void OnBackButtonPress() { }
    }
}
