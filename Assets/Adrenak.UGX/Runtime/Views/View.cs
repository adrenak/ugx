using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Adrenak.UGX {
    /// <summary>
    /// A View with a state object
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(ViewInspectorHelper))]
    public abstract class View<TState> : UGXBehaviour where TState : State {
        /// <summary>
        /// Fired when the <see cref="State"/> field is set
        /// </summary>
        public event EventHandler<TState> Updated;

        /// <summary>
        /// Whether the view should update on start
        /// </summary>
        public bool updateOnStart = true;

        [Tooltip("Current state of the view.")]
        [SerializeField] TState _state;

        bool isBeingDestroyed = false;
        bool initialized = false;

        /// <summary>
        /// Current state of the View
        /// </summary>
        public TState State {
            get => _state;
            set {
                _state = value ?? throw new Exception(nameof(State));
                UpdateView_Internal();
            }
        }

        Dictionary<Func<TState, object>, List<Action<object>>> bindings = new Dictionary<Func<TState, object>, List<Action<object>>>();

        protected void Bind(Func<TState, object> source, Action<object> destination) {
            if (bindings.ContainsKey(source))
                bindings[source].Add(destination);
            else
                bindings.Add(source, new List<Action<object>> { destination });
        }

        /// <summary>
        /// Marked protected to draw attention to the following:
        /// If you are creating a Start() method in your subclass,
        /// be use to call base.Start() inside it. Otherwise Unity
        /// will not call Start() inside View<T>
        /// 
        /// Consider using <see cref="OnInitializeView"/> instead
        /// </summary>
        protected void Start() {
            try {
                OnInitializeView();
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
            initialized = true;
            if (updateOnStart)
                UpdateView_Internal();
        }

        private void OnDestroy() {
            isBeingDestroyed = true;
        }

        /// <summary>
        /// Updates the View using the current state
        /// </summary>
        async void UpdateView_Internal() {
            if (isBeingDestroyed) return;
            await UniTask.WaitWhile(() => !initialized);

#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(gameObject, "Refresh");
#endif
            if (State != null) {
                OnUpdateView();
                Updated?.Invoke(this, _state);
            }
        }

        /// <summary>
        /// Modifies the state and refreshes the view
        /// </summary>
        /// <param name="stateModification">
        /// An Action defining how the state should be modified
        /// </param>
        public void ModifyState(Action<TState> stateModification) {
            stateModification?.Invoke(_state);
            UpdateView_Internal();
        }

        protected abstract void OnInitializeView();

        protected abstract void OnUpdateView();
    }
}