using System;
using System.Collections.Generic;

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
        public RectTransform RectTransform {
            get {
                if (rt == null)
                    rt = GetComponent<RectTransform>();
                return rt;
            }
        }

        /// <summary>
        /// Returns the View on this GameObject, if any
        /// </summary>
        public View<T> GetView<T>() where T : State {
            var v = GetComponent<View<T>>();
            if (v == null) {
                UGX.Debug.LogError("No View component found", gameObject);
                return null;
            }
            return v;
        }

        /// <summary>
        /// Returns the Window on this GameObject, if any
        /// </summary>
        public Window Window {
            get {
                var w = GetComponent<Window>();
                if (w == null) {
                    UGX.Debug.LogError("No Window component found", gameObject);
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
                    UGX.Debug.LogError("No Tweener components found", gameObject);
                    return null;
                }
                return t;
            }
        }

        public EventEmitter EventEmitter {
            get {
                var emitter = GetComponent<EventEmitter>();
                if (emitter == null)
                    emitter = gameObject.AddComponent<EventEmitter>();
                return emitter;
            }
        }

        public EventListener EventListener {
            get {
                var listener = GetComponent<EventListener>();
                if (listener == null)
                    listener = gameObject.AddComponent<EventListener>();
                return listener;
            }
        }
    }
}
