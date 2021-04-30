using System;
using UnityEngine;

namespace Adrenak.UGX {
    [System.Serializable]
    [RequireComponent(typeof(RectTransform))]
    public class UGXBehaviour : MonoBehaviour {
        RectTransform rt;
        /// <summary>
        /// Returns the RectTransform of the GameObject.
        /// </summary>
        public RectTransform RT {
            get {
                if (rt == null)
                    rt = GetComponent<RectTransform>();
                return rt;
            }
        }

        /// <summary>
        /// Returns the View on this GameObject, if any
        /// </summary>
        public View view => GetComponent<View>();

        /// <summary>
        /// Returns the Window on this GameObject, if any
        /// </summary>
        public Window window => GetComponent<Window>();

        /// <summary>
        /// Returns all the Tweeners on this GameObject, if any
        /// </summary>
        public TweenerBase[] tweeners => GetComponents<TweenerBase>();

        [Obsolete("Use .tweeners instead")]
        public TweenerBase[] transitioners => tweeners;
    }
}
