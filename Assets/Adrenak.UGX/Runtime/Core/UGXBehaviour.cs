using System;

using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// The base <see cref="MonoBehaviour"/> class for UGX.
    /// This class mainly provides easy access to UGX specific components
    /// on its <see cref="GameObject"/>
    /// 
    /// For consistency, properties here start with lower-case 
    /// despite C# recommending upper-case because <see cref="Component"/> 
    /// also has properties such as 'transform', 'gameObject'.
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(RectTransform))]
    public abstract class UGXBehaviour : MonoBehaviour {
        RectTransform rt;
        /// <summary>
        /// Returns the RectTransform of the GameObject.
        /// </summary>
        public RectTransform rectTransform {
            get {
                if (rt == null)
                    rt = GetComponent<RectTransform>();
                return rt;
            }
        }

        /// <summary>
        /// Returns the View on this GameObject, if any
        /// </summary>
        public View view {
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
        public Window window {
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
        public TweenerBase[] tweeners {
            get {
                var t = GetComponents<TweenerBase>();
                if (t == null || t.Length == 0) {
                    Debug.LogError("No Tweener components found", gameObject);
                    return null;
                }
                return t;
            }
        }

        public UGXEventEmitter eventEmitter {
            get {
                var emitter = GetComponent<UGXEventEmitter>();
                if (emitter == null)
                    emitter = gameObject.AddComponent<UGXEventEmitter>();
                return emitter;
            }
        }

        public UGXEventListener eventListener {
            get {
                var listener = GetComponent<UGXEventListener>();
                if (listener == null)
                    listener = gameObject.AddComponent<UGXEventListener>();
                return listener;
            }
        }
    }
}
