using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using UnityEngine;
using Object = UnityEngine.Object;

namespace Adrenak.UGX {
    [Serializable]
    [RequireComponent(typeof(RectTransform))]
    public abstract class UGXBehaviour : MonoBehaviour {
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
        public View View {
            get {
                var v = GetComponent<View>();
                if (v == null) {
                    Debug.LogError("No View component found", gameObject);
                    return null;
                }
                return v;
            }
        }

        /// <summary>
        /// Returns the Window on this GameObject, if any
        /// </summary>
        public Window Window {
            get {
                var w = GetComponent<Window>();
                if (w == null) {
                    Debug.LogError("No Window component found", gameObject);
                    return null;
                }
                return w;
            }
        }

        /// <summary>
        /// Returns all the Tweeners on this GameObject, if any
        /// </summary>
        public TweenerBase[] Tweeners {
            get {
                var t = GetComponents<TweenerBase>();
                if (t == null || t.Length == 0) {
                    Debug.LogError("No Tweener components found", gameObject);
                    return null;
                }
                return t;
            }
        }
    }
}
