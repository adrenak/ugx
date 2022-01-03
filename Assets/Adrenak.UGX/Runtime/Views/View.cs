using UnityEngine;
using System;

namespace Adrenak.UGX {
    /// <summary>
    /// The fundamental class used to define anything that is visible
    /// to the user. 
    /// </summary>
    [DisallowMultipleComponent]
    [Serializable]
    [RequireComponent(typeof(RectTransform))]
    public class View : UGXBehaviour {
        /// <summary>
        /// ID that can be used to identify the View
        /// </summary>
        public string ID;

        /// <summary>
        /// Returns a View with the given ID below this view in the heirarchy.
        /// </summary>
        public static View operator /(View me, string childID) {
            var views = me.GetComponentsInChildren<View>();
            foreach (var view in views)
                if (view.ID.Equals(childID))
                    return view;
            return null;
        }
    }
}