using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

namespace Adrenak.UGX {
    [RequireComponent(typeof(RectTransform))]
    public class UGXBehaviour : MonoBehaviour {
        public View view => GetComponent<View>();
        public Window window => GetComponent<Window>();
        public TransitionerBase[] transitioners => GetComponents<TransitionerBase>();        
        public UnityEvent<Visibility> onVisibilityChanged;
        [ReadOnly] [SerializeField] Visibility currentVisibility = Visibility.None;

        public Visibility CurrentVisibility {
            get => currentVisibility;
            private set => currentVisibility = value;
        }

        RectTransform rt;
        public RectTransform RT {
            get {
                if (rt == null)
                    rt = GetComponent<RectTransform>();
                return rt;
            }
        }

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
                return fully.Value ? Visibility.None : Visibility.Partial;
            else
                return Visibility.Full;
        }
    }
}
