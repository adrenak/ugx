using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System;

namespace Adrenak.UGX {
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
}