using System;
using System.Collections.Generic;

using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// Listens for <see cref="UGXEvent"/> being propagated through the scene
    /// hierarchy and allows subscription as well as prevention of further 
    /// propagation of the event in the hierarchy.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UGXEventListener : MonoBehaviour {
        public bool debug;

        List<Func<UGXEvent, bool>> subscribers = 
            new List<Func<UGXEvent, bool>>();

        /// <summary>
        /// Subscribes to any event received by this listener and returns
        /// if the propagation should continue.
        /// </summary>
        /// <param name="handler">The handler </param>
        public void Subscribe(Func<UGXEvent, bool> handler) {
            subscribers.Add(handler);
            if (debug) {
                var count = subscribers.Count;
                string msg = $"New Subscription. Total subscribers: {count}";
                Debug.Log(msg, gameObject);
            }
        }

        public void Unsubscribe(Func<UGXEvent, bool> handler) {
            if (!subscribers.Contains(handler)) return;
            
            subscribers.Remove(handler);
            if(debug) {
                var count = subscribers.Count;
                string msg = $"Subscription removed. Total subscribers: {count}";
                Debug.Log(msg, gameObject);
            }
        }

        /// <summary>
        /// Notifies the subscribers about this event and returns
        /// if subscribers want this event to continue propagating
        /// in the scene hierarchy.
        /// </summary>
        /// <param name="ugxEvent">The event instance</param>
        /// <returns>Whether the event should continue propagating</returns>
        public bool Notify(UGXEvent ugxEvent) {
            if (debug) {
                string msg = $"Received event: {ugxEvent}";
                Debug.Log(msg, gameObject);
            }

            // Notify all subscribers and check if any of the
            // handlers want to prevent propagation
            if (subscribers.Count > 0) {
                bool propagate = true;
                foreach (var handler in subscribers) {
                    var response = handler(ugxEvent);
                    if (!response)
                        propagate = false;
                }
                return propagate;
            }
            return true;
        }
    }
}
