using System;
using UnityEngine;

namespace Adrenak.UPF {
    [Serializable]
    public abstract class View<TViewModel> : View where TViewModel : ViewModel {
        public event EventHandler<TViewModel> OnViewModelSet;
        public event EventHandler<string> OnViewModelModified;
        [SerializeField] bool refreshOnStart = true;

        [SerializeField] TViewModel vm;
        public TViewModel ViewModel {
            get => vm;
            set {
                vm = value ?? throw new ArgumentNullException(nameof(ViewModel));

                vm.PropertyChanged += (sender, e) => {
                    HandleViewModelModification(e.PropertyName);
                    OnViewModelModified?.Invoke(this, e.PropertyName);
                };
                OnViewModelSet?.Invoke(this, vm);                
                HandleViewModelSet();
            }
        }

        void Awake() {
            InitializeView();
            vm.PropertyChanged += (sender, e) => {
                HandleViewModelModification(e.PropertyName);
                OnViewModelModified?.Invoke(this, e.PropertyName);
            };
            ObserveView();

            if (refreshOnStart)
                HandleViewModelSet();
        }

        protected abstract void InitializeView();
        protected abstract void HandleViewModelSet();
        protected abstract void ObserveView();
        protected abstract void HandleViewModelModification(string propertyName);
    }

    [Serializable]
    public class View : BindableBehaviour {
        public event EventHandler<Visibility> OnVisibilityChanged;
        public event EventHandler OnViewDestroyed;

        public Visibility CurrentVisibility { get; private set; } = Visibility.None;

        RectTransform rt;
        RectTransform RT {
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
            OnViewDestroyed?.Invoke(this, EventArgs.Empty);
        }

        void Update() {
            UpdateVisibility();
        }

        void UpdateVisibility() {
            var visibility = GetVisibility();
            if (CurrentVisibility == visibility) return;

            CurrentVisibility = visibility;
            OnVisibilityChanged?.Invoke(this, CurrentVisibility);
        }

        Visibility GetVisibility() {
            if (RT.IsVisible(out bool? fully))
                return fully.Value ? Visibility.None : Visibility.Partial;
            else
                return Visibility.Full;
        }
    }
}