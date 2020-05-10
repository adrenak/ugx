using UnityEngine;
using UnityWeld.Binding;

namespace Adrenak.UPF {
    //[Binding]
    //public class View<T> : BindableBehaviour where T : ViewModel {
    //    [SerializeField] T _model;
    //    [Binding]
    //    public T Model {
    //        get => _model;
    //        set => Set(ref _model, value);
    //    }
    //}

    [Binding]
    public class View<T> : View where T : ViewModel {
        [SerializeField] T _model;
        [Binding]
        public T Model {
            get => _model;
            set => Set(ref _model, value);
        }
    }

    public class View : BindableBehaviour { }
}
