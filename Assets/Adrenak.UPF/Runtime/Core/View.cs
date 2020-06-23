using System;
using UnityEngine;

namespace Adrenak.UPF {
    [Serializable]
    public abstract class View<TViewModel> : View where TViewModel : ViewModel {
        public event EventHandler<TViewModel> OnViewModelSet;
        [SerializeField] bool refreshOnStart = true;

        [SerializeField] TViewModel _vm;
        public TViewModel ViewModel {
            get => _vm;
            set {
                _vm = value ?? throw new ArgumentNullException(nameof(ViewModel));
                Refresh();
                OnViewModelSet?.Invoke(this, _vm);
                _vm.PropertyChanged += (sender, e) => ObserveModel(e.PropertyName);
            }
        }

        void Awake() {
            InitializeView();
            _vm.PropertyChanged += (sender, e) => ObserveModel(e.PropertyName);

            if(refreshOnStart)
                Refresh();
            ObserveView();
        }

        protected abstract void InitializeView();
        protected abstract void Refresh();
        protected abstract void ObserveView();
        protected abstract void ObserveModel(string propertyName);
    }

    [Serializable]
    public class View : BindableBehaviour {
        public event EventHandler OnViewDestroyed;

        public View GetSubView(string subViewName) {
            for (int i = 0; i < transform.childCount; i++) {
                var child = transform.GetChild(i);
                if (child.gameObject.name.Equals(subViewName)) {
                    var view = child.GetComponent<View>();
                    if (view != null)
                        return view;
                }
            }
            throw new Exception($"No GameObject named {subViewName} with View component was found under {gameObject.name}");
        }

        void OnDestroy() {
            OnViewDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
