using System;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using Adrenak.Unex;

namespace Adrenak.UPF {
    [Serializable]
    public abstract class View<TViewModel> : View where TViewModel : ViewModel {
        public event EventHandler<TViewModel> ViewModelSet;

        [BoxGroup("---VIEW---")] [SerializeField] TViewModel viewModel;
        public TViewModel ViewModel {
            get => viewModel;
            set {
                viewModel = value ?? throw new ArgumentNullException(nameof(ViewModel));
                ViewModelSet?.Invoke(this, viewModel);
                OnViewModelSet();
            }
        }

        [Button]
        public void Refresh() => OnViewModelSet();

        [Button]
        public void Clear() => ViewModel = default;
        
        protected abstract void OnViewModelSet();
    }

    [Serializable]
    public class View : BindableBehaviour {
        [BoxGroup("---VIEW---")] [SerializeField] bool showEvents;
        [BoxGroup("---VIEW---")] [ShowIf("showEvents")] public UnityEvent onViewOpen;
        [BoxGroup("---VIEW---")] [ShowIf("showEvents")] public UnityEvent onViewClose;
        [BoxGroup("---VIEW---")] [ShowIf("showEvents")] public UnityEvent onPressBack;
        [BoxGroup("---VIEW---")] [ShowIf("showEvents")] public UnityEvent onViewDestroyed;
        public UnityEvent<Visibility> onVisibilityChanged;

        public bool IsDestroyed { get; private set; } = false;
        public bool IsOpen => isOpen;
        public Visibility CurrentVisibility { get; private set; } = Visibility.None;

        public RectTransform RectTransform => rt ?? (rt = GetComponent<RectTransform>());

        [ReadOnly] [BoxGroup("---VIEW---")] [SerializeField] bool isOpen;
        
        RectTransform rt;
        bool isOpening, isClosing;

        public void OpenView(bool silently = false) {
            if (isOpen || isOpening) return;

            isOpening = true;
            Dispatcher.Enqueue(() => {
                isOpen = true;
                isOpening = false;
            });

            OnViewOpen();
            if (!silently)
                onViewOpen?.Invoke();
        }

        public void CloseView(bool silently = false) {
            if (!isOpen || isClosing) return;

            isClosing = true;
            Dispatcher.Enqueue(() => {
                isOpen = false;
                isClosing = false;
            });

            OnViewClose();
            if (!silently)
                onViewClose?.Invoke();
        }

        void Start() {
            CurrentVisibility = GetVisibility();
        }

        /// <summary>
        /// If you're overriding this, make sure to call base.Update() in your subclass
        /// </summary>
        protected void Update() {
            CheckBackPress();
            UpdateVisibility();
        }

        void OnDestroy() {
            IsDestroyed = true;
            onViewDestroyed?.Invoke();
        }

        void CheckBackPress() {
            if (Input.GetKeyUp(KeyCode.Escape) && IsOpen) {
                onPressBack?.Invoke();
                OnPressBack();
            }
        }

        void UpdateVisibility() {
            var visibility = GetVisibility();
            if (CurrentVisibility == visibility) return;

            CurrentVisibility = visibility;
            onVisibilityChanged?.Invoke(CurrentVisibility);
        }

        Visibility GetVisibility() {
            if (RectTransform.IsVisible(out bool? fully))
                return fully.Value ? Visibility.None : Visibility.Partial;
            else
                return Visibility.Full;
        }

        protected virtual void OnViewOpen() { }
        protected virtual void OnViewClose() { }
        protected virtual void OnPressBack() { }
    }
}