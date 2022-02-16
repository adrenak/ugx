using UnityEngine;
using System;
using System.Linq;
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
                if(value == null) {
                    Debug.LogError("Cannot set State to null");
                    return;
                }

                // Update the listeners of the binding sources that
                // have changed after state object set
                TriggerBindingsIfChanged(_state, value);

                _state = value;
                UpdateView_Internal();
            }
        }

        List<object> GetBindingSources(TState state) =>
            bindings.Keys.Select(x => x(state)).ToList();

        void TriggerBindingsIfChanged(TState oldState, TState newState) {
            foreach (var binding in bindings) {
                var oldSource = binding.Key(oldState);
                var newSource = binding.Key(newState);
                if (!oldSource.Equals(newSource))
                    foreach (var destination in binding.Value)
                        destination(newSource);
            }
        }

        void TriggerAllBindings() {
            foreach(var binding in bindings) {
                var value = binding.Key(State);
                foreach (var destination in binding.Value)
                    destination(value);
            }
        }

        SortedDictionary<Func<TState, object>, List<Action<object>>> bindings = new SortedDictionary<Func<TState, object>, List<Action<object>>>();

        protected void Bind(Func<TState, object> source, Action<object> listener) {
            if (bindings.ContainsKey(source))
                bindings[source].Add(listener);
            else
                bindings.Add(source, new List<Action<object>> { listener });
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

        private void OnValidate() {
            TriggerAllBindings();
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
                //TriggerAllBindings();
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
            var oldState = _state.ShallowClone();
            stateModification?.Invoke(_state);
            TriggerBindingsIfChanged(oldState, _state);
            UpdateView_Internal();
        }

        protected abstract void OnInitializeView();

        protected abstract void OnUpdateView();
    }
}