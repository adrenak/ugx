using UnityEngine;
using NaughtyAttributes;
using System;

namespace Adrenak.UGX {
    /// <summary>
    /// A View with a state object
    /// </summary>
    [Serializable]
    public abstract class StatefulView<TState> : View where TState : ViewState {
        /// <summary>
        /// Fired when the <see cref="State"/> field is set
        /// </summary>
        public event EventHandler<TState> StateSet;

        /// <summary>
        /// Whether the View should update itself with the state it starts with.
        /// </summary>
        [Tooltip("Whether the View should update itself with the state it starts with.")]
        [BoxGroup("View State")] public bool updateFromStateOnStart = false;

        [Tooltip("Current state of the view. Changing values inside it will not trigger Update" + 
        "automatically. You must click the Update View button on this component to see the changes.")]
        [BoxGroup("View State")] [SerializeField] TState state;

        /// <summary>
        /// Current state of the View
        /// </summary>
        public TState State {
            get => state;
            set {
                state = value ?? throw new Exception(nameof(State));
                StateSet?.Invoke(this, state);
                OnStateSet();
            }
        }

        /// <summary>
        /// If you inherit from this class and create a Start() method there,
        /// make sure the first line of that method is base.Start() so that
        /// StatefulView.Start() is called
        /// </summary>
        protected void Start() {
            if (updateFromStateOnStart)
                UpdateView();

            try {
                OnStart();
            }
            catch { }
        }

        /// <summary>
        /// Updates the View using the current state
        /// </summary>
        [Button("Update View")]
        public void UpdateView() {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(gameObject, "Update View");
#endif
            OnStateSet();
        }

        /// <summary>
        /// Modifies the state based on the action injected and Updates the view
        /// </summary>
        /// <param name="stateModification">An Action defining how the state should be modified</param>
        public void UpdateView(Action<TState> stateModification) {
            stateModification?.Invoke(state);
            UpdateView();
        }

        /// <summary>
        /// Sets the state and Updates the view
        /// </summary>
        /// <param name="state">The new state to be used</param>
        public void UpdateView(TState state){
            State = state;
            UpdateView();
        }

        protected abstract void OnStart();

        protected abstract void OnStateSet();
    }
}