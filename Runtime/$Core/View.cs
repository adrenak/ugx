using System;
using UnityEngine;

namespace Adrenak.UPF {
    [Serializable]
    public abstract class View<T> : View where T : Model {
        [SerializeField] T _model;
        public T Model {
            get => _model;
            set {
                _model = value ?? throw new ArgumentNullException(nameof(Model));
                Refresh();
            }
        }

        void Start() {
            ObserveView();
            Init();
        }

        protected abstract void Refresh();
        protected abstract void ObserveView();
        protected abstract void ObserveModel(string propertyName);
    }

    [Serializable]
    public class View : BindableBehaviour {
        public event EventHandler OnViewSelected;
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

        public void Select() {
            OnViewSelected?.Invoke(this, EventArgs.Empty);
        }

        void OnDestroy() {
            OnViewDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
