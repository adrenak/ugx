using UnityEngine;
using System;

namespace Adrenak.UGX {
    /// <summary>
    /// A View with a state object
    /// </summary>
    [Serializable]
    public abstract class View<T> : UGXBehaviour where T : State {
        /// <summary>
        /// Fired when the <see cref="State"/> field is set
        /// </summary>
        public event EventHandler<T> ViewStateUpdated;

        [Tooltip("Current state of the view.")]
        [SerializeField] T _state;

        /// <summary>
        /// Current state of the View
        /// </summary>
        public T State {
            get => _state;
            set {
                if(value == null) {
                    Debug.LogError("Cannot set State to null");
                    return;
                }

                _state = value;
                UpdateView_Internal();
            }
        }

        /// <summary>
        /// Invoked to reset the View's UI and state.
        /// Also, use this to provide a method to "cleanup" the
        /// View so that it can be used in a ListView
        /// </summary>
        public virtual void ResetView() { }

        /// <summary>
        /// Called by View<T> when the View State has been
        /// swapped or modified.
        /// </summary>
        protected virtual void OnViewStateChange() { }

        /// <summary>
        /// Modifies the state and refreshes the view
        /// </summary>
        /// <param name="stateModification">
        /// An Action defining how the state should be modified
        /// </param>
        public void ModifyState(Action<T> stateModification) {
            stateModification?.Invoke(_state);
            UpdateView_Internal();
        }

        /// <summary>
        /// Updates the View using the current state
        /// </summary>
        void UpdateView_Internal() {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(gameObject, "UpdateView");
#endif
            if (State != null) {
                OnViewStateChange();
                ViewStateUpdated?.Invoke(this, _state);
            }
        }
    }
}