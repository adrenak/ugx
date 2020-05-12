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
    public class View : BindableBehaviour {
        public event EventHandler Destroyed;

        void OnDestroy() {
            Destroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
