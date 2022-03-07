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
        long age;

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

                _state = value;
                UpdateView_Internal();
            }
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

        protected void Update() {
            age++;
        }

        private void OnDestroy() {
            isBeingDestroyed = true;
        }

        private void OnValidate() {
            try {
                if(age > 1)
                    OnUpdateView();
            }
            catch { }
        }

        /// <summary>
        /// Updates the View using the current state
        /// </summary>
        async void UpdateView_Internal() {
            if (isBeingDestroyed) return;
            await UniTask.WaitWhile(() => !initialized);

#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(gameObject, "UpdateView");
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