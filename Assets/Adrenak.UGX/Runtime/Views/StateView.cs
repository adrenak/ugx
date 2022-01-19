using UnityEngine;
using System;

namespace Adrenak.UGX {
    /// <summary>
    /// A View with a state object
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(StateViewRefresher))]
    public abstract class StateView<TState> : View where TState : State {
        /// <summary>
        /// Fired when the <see cref="State"/> field is set
        /// </summary>
        public event EventHandler<TState> Refreshed;

        /// <summary>
        /// Whether the View should refresh on start.
        /// </summary>
        [Tooltip("Whether the View should refresh on start.")]
        public bool refreshOnStart = false;

        /// <summary>
        /// Whether the View should refresh when the state object 
        /// is changed to a new object.
        /// </summary>
        [Tooltip("Whether the View should refresh when the state object " +
        "is changed to a new object.")]
        public bool refreshOnStateSwap = true;

        [Tooltip("Current state of the view.")]
        [SerializeField] TState _state;

        /// <summary>
        /// Current state of the View
        /// </summary>
        public TState State {
            get => _state;
            set {
                _state = value ?? throw new Exception(nameof(State));
                if (refreshOnStateSwap)
                    RefreshFromState();
            }
        }

        /// <summary>
        /// Consider using <see cref="OnStart"/> instead of Start when 
        /// inheriting this class.
        /// 
        /// The way that Unity event methods work causes the Start
        /// method in the base class to not be called. So, in inheriting,
        /// call base.Start() in the first line.
        /// </summary>
        protected void Start() {
            if (refreshOnStart)
                RefreshFromState();

            OnStart();
        }

        /// <summary>
        /// Updates the View using the current state
        /// </summary>
        public void RefreshFromState() {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(gameObject, "Refresh");
#endif
            OnRefresh();
            Refreshed?.Invoke(this, _state);
        }

        /// <summary>
        /// Modifies the state and refreshes the view
        /// </summary>
        /// <param name="stateModification">
        /// An Action defining how the state should be modified
        /// </param>
        public void Refresh(Action<TState> stateModification) {
            stateModification?.Invoke(_state);
            RefreshFromState();
        }

        /// <summary>
        /// Sets the state and refreshes the view
        /// </summary>
        /// <param name="state">The new state to be used</param>
        public void Refresh(TState state) {
            this.State = state;

            // only if auto refresh is off we refresh manually
            if (!refreshOnStateSwap)
                RefreshFromState();
        }

        protected abstract void OnStart();

        protected abstract void OnRefresh();
    }
}