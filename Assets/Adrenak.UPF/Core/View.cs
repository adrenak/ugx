using System;
using UnityEngine;

namespace Adrenak.UPF {
    [Serializable]
    public abstract class View<T> : View where T : ViewModel {
        [SerializeField] T _bindingContext;
        public T Context {
            get => _bindingContext;
            set {
                Set(ref _bindingContext, value);
                InitializeFromContext();
            }
        }

        void Awake() {
            InitializeFromContext();
            BindViewToContext();
            Context.PropertyChanged += (sender, args) => OnPropertyChange(args.PropertyName);
        }

        protected abstract void InitializeFromContext();
        protected abstract void OnPropertyChange(string propertyName);
        protected abstract void BindViewToContext();
    }

    [Serializable]
    [DisallowMultipleComponent]
    public class View : BindableBehaviour {
        public event EventHandler OnViewSelected;
        public event EventHandler OnViewDestroyed;

        public View GetSubView(string subViewName) {
            for(int i = 0; i < transform.childCount; i++){
                var child = transform.GetChild(i);
                if(child.gameObject.name.Equals(subViewName)){
                    var view = child.GetComponent<View>();
                    if (view != null)
                        return view;
                }
            }
            throw new Exception($"No GameObject named {subViewName} with View component was found under {gameObject.name}");
        }

        public void Select() {
            OnViewSelected?.Invoke(this, EventArgs.Empty);
        }

        void OnDestroy() {
            OnViewDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
