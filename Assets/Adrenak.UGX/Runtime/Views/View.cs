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
    public abstract class View<T> : UGXBehaviour where T : State {
        /// <summary>
        /// Fired when the <see cref="State"/> field is set
        /// </summary>
        public event EventHandler<T> ViewStateUpdated;

        public bool IsViewInitialized { get; private set; } = false;

        [SerializeField] bool autoInitilalizeOnStart = true;
        public bool AutoInitializeOnStart {
            get => autoInitilalizeOnStart;
            set => autoInitilalizeOnStart = value;
        }
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
        /// Marked protected to draw attention to the following:
        /// If you are creating a Start() method in your subclass,
        /// be sure to call base.Start() inside it in the first line. 
        /// 
        /// Consider using <see cref="OnInitializeView"/> instead
        /// </summary>
        protected void Start() {
            try {
                if(AutoInitializeOnStart) {
                    OnInitializeView();
                    IsViewInitialized = true;
                }
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
        }

        private void OnValidate() {
            try {
                if(!Application.isPlaying)
                    OnStateChange();
            }
            catch { }
        }

        /// <summary>
        /// Updates the View using the current state
        /// </summary>
        void UpdateView_Internal() {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(gameObject, "UpdateView");
#endif
            if (State != null) {
                OnStateChange();
                ViewStateUpdated?.Invoke(this, _state);
            }
        }

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

        protected abstract void OnInitializeView();

        protected abstract void OnStateChange();
    }
}