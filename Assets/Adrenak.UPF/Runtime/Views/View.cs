using System;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using Adrenak.Unex;

namespace Adrenak.UPF {
    [Serializable]
    public abstract class View<TViewModel> : View where TViewModel : ViewModel {
        public event EventHandler<TViewModel> ViewModelSet;
        public event EventHandler<string> ViewModelModified;
                
        [BoxGroup("Data")] [SerializeField] TViewModel viewModel;
        public TViewModel ViewModel {
            get => viewModel;
            set {
                viewModel = value ?? throw new ArgumentNullException(nameof(ViewModel));

                viewModel.PropertyChanged += (sender, e) => {
                    OnViewModelModified(e.PropertyName);
                    ViewModelModified?.Invoke(this, e.PropertyName);
                };
                ViewModelSet?.Invoke(this, viewModel);
                OnViewModelSet();
            }
        }

        protected void Awake() {
            viewModel.PropertyChanged += (sender, e) => {
                OnViewModelModified(e.PropertyName);
                ViewModelModified?.Invoke(this, e.PropertyName);
            };
            OnViewModelSet();
        }

        protected abstract void OnViewModelSet();
        protected abstract void OnViewModelModified(string propertyName);
    }

    [Serializable]
    public class BlankViewModel : ViewModel { }

    [Serializable]
    public class View : BindableBehaviour {
        [BoxGroup("Events")] [SerializeField] bool showEvents;
        [BoxGroup("Events")] [ShowIf("showEvents")] public UnityEvent onPageOpen;
        [BoxGroup("Events")] [ShowIf("showEvents")] public UnityEvent onPageClose;
        [BoxGroup("Events")] [ShowIf("showEvents")] public UnityEvent onPressBack;
        [BoxGroup("Events")] [ShowIf("showEvents")] public UnityEvent onViewDestroyed;
        public UnityEvent<Visibility> onVisibilityChanged;

        public bool IsDestroyed { get; private set; } = false;
        public bool IsOpen => isOpen;
        public Visibility CurrentVisibility { get; private set; } = Visibility.None;

        public RectTransform RectTransform => rt ?? (rt = GetComponent<RectTransform>());

        [BoxGroup("Navigation")] [SerializeField] protected Navigator navigator;
        [ReadOnly] [BoxGroup("Navigation")] [SerializeField] bool isOpen;
        
        RectTransform rt;
        bool isOpening, isClosing;

        public void OpenPage(bool silently = false) {
            if (isOpen || isOpening) return;

            isOpening = true;
            Dispatcher.Enqueue(() => {
                isOpen = true;
                isOpening = false;
            });

            OnPageOpen();
            if (!silently)
                onPageOpen?.Invoke();
        }

        public void ClosePage(bool silently = false) {
            if (!isOpen || isClosing) return;

            isClosing = true;
            Dispatcher.Enqueue(() => {
                isOpen = false;
                isClosing = false;
            });

            OnPageClose();
            if (!silently)
                onPageClose?.Invoke();
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
                navigator.Pop();
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

        protected virtual void OnPageOpen() { }
        protected virtual void OnPageClose() { }
        protected virtual void OnPressBack() { }
    }
}