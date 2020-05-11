using System;
using UnityEngine;
using UnityWeld.Binding;

namespace Adrenak.UPF {
    [Binding]
    public class View<T> : View where T : ViewModel {
        [SerializeField] T _bindingContext;
        [Binding]
        public T BindingContext {
            get => _bindingContext;
            set => Set(ref _bindingContext, value);
        }
    }

    public class View : BindableBehaviour {
        public event EventHandler Destroyed;

        void OnDestroy() {
            Destroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
