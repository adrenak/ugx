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

        protected abstract void InitializeFromContext();
        protected abstract void OnPropertyChange(string propertyName);
        protected abstract void BindViewToContext();

        void Awake() {
            InitializeFromContext();
            BindViewToContext();
            Context.PropertyChanged += (sender, args) => OnPropertyChange(args.PropertyName);
        }
    }

    [Serializable]
    [DisallowMultipleComponent]
    public class View : BindableBehaviour {
        public event EventHandler Destroyed;

        public View GetSubView(string gameObjectName) {
            for(int i = 0; i < transform.childCount; i++){
                var child = transform.GetChild(i);
                if(child.gameObject.name.Equals(gameObjectName)){
                    var view = child.GetComponent<View>();
                    if (view != null)
                        return view;
                }
            }
            return null;
        }

        void OnDestroy() {
            Destroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
