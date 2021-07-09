using UnityEngine;
using System;

namespace Adrenak.UGX {
    public abstract class View<T> : View where T : ViewModel {
        /// <summary>
        /// Fired when the <see cref="Model"/> field is set
        /// </summary>
        public event EventHandler<T> ViewModelSet;

        [Tooltip("Current model of the view")]
        [SerializeField] T model;

        protected abstract void OnViewModelUpdate();

        /// <summary>
        /// Current state of the View
        /// </summary>
        public T Model {
            get => model;
            set {
                model = value ?? throw new Exception(nameof(Model));
                ViewModelSet?.Invoke(this, model);
                OnViewModelUpdate();
            }
        }

        /// <summary>
        /// Updates the View using the current state
        /// </summary>
        [ContextMenu("Update View")]
        public void UpdateView() {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(gameObject, "Update View");
#endif
            OnViewModelUpdate();
        }

        /// <summary>
        /// Modifies the VM based on the action injected and Updates the view
        /// </summary>
        /// <param name="viewModelAccess">
        /// An Action that gives access to the ViewModel instace.
        /// </param>
        public void ModifyViewModel(Action<T> viewModelAccess) {
            viewModelAccess?.Invoke(model);
            UpdateView();
        }

        [Obsolete("Use Model now")]
        public T State {
            get => Model;
            set => Model = value;
        }

        [Obsolete("Use OnUpdateView instead", true)]
        protected virtual void OnStateSet() { }
    }

    /// <summary>
    /// A View with a model object
    /// </summary>
    [Serializable]
    [Obsolete]
    public abstract class StatefulView<TState> :
        View<TState> where TState : ViewModel { }
}