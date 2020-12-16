using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System;

namespace Adrenak.UGX {
    [Serializable]
    public abstract class ViewModel {
        public string ID = Guid.NewGuid().ToString();
    }

    [Serializable]
    public abstract class View<TViewModel> : View where TViewModel : ViewModel {
        public event EventHandler<TViewModel> ViewModelSet;

        [SerializeField] bool autoStart = false;

        [SerializeField] TViewModel viewModel;
        public TViewModel ViewModel {
            get => viewModel;
            set {
                viewModel = value ?? throw new ArgumentNullException(nameof(ViewModel));
                ViewModelSet?.Invoke(this, viewModel);
                OnViewModelSet();
            }
        }

        protected new void Start() {
            base.Start();
            if (autoStart)
                OnViewModelSet();
        }

        [Button]
        public void Refresh() => OnViewModelSet();

        [Button]
        public void Clear() => ViewModel = default;

        protected abstract void OnViewModelSet();
        protected virtual void OnViewModelModify(string name) { }
    }

    [Serializable]
    [RequireComponent(typeof(RectTransform))]
    public class View : UIBehaviour {
        public UnityEvent<Visibility> onVisibilityChanged;
        [ReadOnly] [SerializeField] Visibility currentVisibility = Visibility.None;

        public Visibility CurrentVisibility {
            get => currentVisibility;
            private set => currentVisibility = value;
        }

        RectTransform rt;
        public RectTransform RectTransform => rt ?? (rt = GetComponent<RectTransform>());

        /// <summary>
        /// If you're overriding this, make sure to call base.Start() first thing
        /// in the Start method of your subclass
        /// By marking this method as protected, it'll warn any inheritors
        /// </summary>
        protected void Start() => CurrentVisibility = GetVisibility();

        /// <summary>
        /// If you're overriding this, make sure to call base.Update() first thing
        /// in the Update method of your subclass
        /// By marking this method as protected, it'll warn any inheritors
        /// </summary>
        protected void Update() {
            UpdateVisibility();
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
    }
}
