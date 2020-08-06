using System;
using UnityEngine;

namespace Adrenak.UPF {
    [Serializable]
    public abstract class View<TViewModel> : View where TViewModel : ViewModel {
        public event EventHandler<TViewModel> ViewModelSet;
        public event EventHandler<string> ViewModelModified;
        [SerializeField] bool refreshOnStart = true;

        [SerializeField] TViewModel vm;
        public TViewModel ViewModel {
            get => vm;
            set {
                vm = value ?? throw new ArgumentNullException(nameof(ViewModel));

                vm.PropertyChanged += (sender, e) => {
                    OnViewModelModified(e.PropertyName);
                    ViewModelModified?.Invoke(this, e.PropertyName);
                };
                ViewModelSet?.Invoke(this, vm);
                OnViewModelSet();
            }
        }

        void Awake() {
            OnViewAwake();
            vm.PropertyChanged += (sender, e) => {
                OnViewModelModified(e.PropertyName);
                ViewModelModified?.Invoke(this, e.PropertyName);
            };

            if (refreshOnStart)
                OnViewModelSet();
        }

        protected abstract void OnViewAwake();
        protected abstract void OnViewModelSet();
        protected abstract void OnViewModelModified(string propertyName);
    }

    [Serializable]
    public class View : BindableBehaviour {
        public event EventHandler<Visibility> VisibilityChanged;
        public event EventHandler ViewDestroyed;

        public Visibility CurrentVisibility { get; private set; } = Visibility.None;
        public bool IsDestroyed { get; private set; } = false;

        RectTransform rt;
        RectTransform RectTransform {
            get {
                if (rt == null)
                    rt = GetComponent<RectTransform>();
                return rt;
            }
        }

        void Start() {
            CurrentVisibility = GetVisibility();
        }

        void OnDestroy() {
            IsDestroyed = true;
            ViewDestroyed?.Invoke(this, EventArgs.Empty);
        }

        void Update() {
            UpdateVisibility();
        }

        void UpdateVisibility() {
            var visibility = GetVisibility();
            if (CurrentVisibility == visibility) return;

            CurrentVisibility = visibility;
            VisibilityChanged?.Invoke(this, CurrentVisibility);
        }

        Visibility GetVisibility() {
            if (RectTransform.IsVisible(out bool? fully))
                return fully.Value ? Visibility.None : Visibility.Partial;
            else
                return Visibility.Full;
        }
    }
}