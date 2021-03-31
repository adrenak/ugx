using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System;

namespace Adrenak.UGX {
    [Serializable]
    public abstract class ViewState {
        public string ID = Guid.NewGuid().ToString();
    }

    [Serializable]
    public abstract class View<TViewState> : View where TViewState : ViewState {
        public event EventHandler<TViewState> OnViewStateSet;

        public bool refreshOnAwake = false;

        [SerializeField] TViewState myViewState;
        public TViewState MyViewState {
            get => myViewState;
            set {
                myViewState = value ?? throw new ArgumentNullException(nameof(MyViewState));
                OnViewStateSet?.Invoke(this, myViewState);
                HandleViewStateSet();
            }
        }

        protected new void Awake() {
            base.Awake();
            if (refreshOnAwake)
                Refresh();
        }

        [Button]
        public void Refresh() => HandleViewStateSet();

        [Button]
        public void Clear() => MyViewState = default;

        protected abstract void HandleViewStateSet();
    }

    [Serializable]
    [RequireComponent(typeof(RectTransform))]
    public class View : UGXBehaviour { }
}
