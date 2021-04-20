using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System;

namespace Adrenak.UGX {
    [DisallowMultipleComponent]
    [Serializable]
    [RequireComponent(typeof(RectTransform))]
    public class View : UGXBehaviour {
        public string viewID = Guid.NewGuid().ToString();
        public UnityEvent<Visibility> onVisibilityChanged;

        [ReadOnly] [SerializeField] Visibility currentVisibility = Visibility.None;

        public static View operator / (View S1, string childName) {
            var views = S1.GetComponentsInChildren<View>();
            foreach (var view in views)
                if (view.viewID.Equals(childName))
                    return view;
            return null;
        }

        public Visibility CurrentVisibility {
            get => currentVisibility;
            private set => currentVisibility = value;
        }

        public T As<T>() where T : View => this as T;

        /// <summary>
        /// If you're overriding this, make sure to call base.Awake() first thing
        /// in the Awake method of your subclass
        /// By marking this method as protected, it'll warn any inheritors
        /// </summary>
        protected void Awake() => CurrentVisibility = GetVisibility();

        /// <summary>
        /// If you're overriding this, make sure to call base.Update() first thing
        /// in the Update method of your subclass
        /// By marking this method as protected, it'll warn any inheritors
        /// </summary>
        protected void Update() {
            UpdateVisibility();
        }

        void UpdateVisibility() {
            var visibility = GetVisibility();
            if (CurrentVisibility == visibility) return;

            CurrentVisibility = visibility;
            onVisibilityChanged?.Invoke(CurrentVisibility);
        }

        Visibility GetVisibility() {
            if (RT.IsVisible(out bool? fully))
                return fully.Value ? Visibility.Full : Visibility.Partial;
            else
                return Visibility.None;
        }
    }
}