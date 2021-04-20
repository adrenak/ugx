using UnityEngine;
using System.Collections;
using NaughtyAttributes;
using System;

namespace Adrenak.UGX {
    [Serializable]
    public abstract class StatefulView<TState> : View where TState : State {
        public event EventHandler<TState> OnViewStateSet;

        [BoxGroup("View State")] public bool updateFromStateOnStart = false;

        [BoxGroup("View State")] [SerializeField] TState currentState;
        public TState CurrentState {
            get => currentState;
            set {
                currentState = value ?? throw new Exception(nameof(CurrentState));
                OnViewStateSet?.Invoke(this, currentState);
                HandleStateSet();
            }
        }

        protected void Start() {
            if (updateFromStateOnStart) 
                UpdateView();
        }

        [Button("Update View")]
        public void UpdateView() {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(gameObject, "Update View");
#endif
            HandleStateSet();
        }

        protected abstract void HandleStateSet();
    }
}