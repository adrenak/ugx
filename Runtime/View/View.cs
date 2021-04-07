using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System;

namespace Adrenak.UGX {
    [Serializable]
    public abstract class ViewState {
        public string ID = Guid.NewGuid().ToString();
    }

    [Serializable]
    public abstract class View<TViewState> : View where TViewState : ViewState {
        public event EventHandler<TViewState> OnViewStateSet;

        [BoxGroup("View State")] public bool updateFromStateOnStart = false;

        [BoxGroup("View State")] [SerializeField] TViewState currentState;
        public TViewState CurrentState {
            get => currentState;
            set {
                currentState = value ?? throw new Exception(nameof(CurrentState));
                OnViewStateSet?.Invoke(this, currentState);
                HandleViewStateSet();
            }
        }

        void Start() {
            if (updateFromStateOnStart)
                UpdateFromState();
        }

        [Button("Update View")]
        public void UpdateFromState() {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(gameObject, "Update View");
#endif
            HandleViewStateSet();
        }

        protected abstract void HandleViewStateSet();
    }

    [DisallowMultipleComponent]
    [Serializable]
    [RequireComponent(typeof(RectTransform))]
    public class View : UGXBehaviour {
        public UnityEvent<Visibility> onVisibilityChanged;
        [ReadOnly] [SerializeField] Visibility currentVisibility = Visibility.None;

        public Visibility CurrentVisibility {
            get => currentVisibility;
            private set => currentVisibility = value;
        }

        /// <summary>
        /// If you're overriding this, make sure to call base.Awake() first thing
        /// in the Awake method of your subclass
        /// By marking this method as protected, it'll warn any inheritors
        /// </summary>
        protected void Awake() => CurrentVisibility = GetVisibility();

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
            if (RT.IsVisible(out bool? fully))
                return fully.Value ? Visibility.None : Visibility.Partial;
            else
                return Visibility.Full;
        }
    }
}