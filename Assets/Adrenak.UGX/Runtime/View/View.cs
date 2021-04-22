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
    }
}