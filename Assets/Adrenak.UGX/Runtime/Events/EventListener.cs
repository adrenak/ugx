using System;
using System.Collections.Generic;

using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// Listens for <see cref="Event"/> being propagated through the scene
    /// hierarchy and allows subscription as well as prevention of further 
    /// propagation of the event in the hierarchy.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class EventListener : UGXBehaviour {
        

        public bool debug;

        List<Func<Event, bool>> subscribers =
            new List<Func<Event, bool>>();

        /// <summary>
        /// Subscribes to any event received by this listener and returns
        /// if the propagation should continue.
        /// </summary>
        /// <param name="handler">The handler </param>
        public void Subscribe(Func<Event, bool> handler) {
            subscribers.Add(handler);
            if (debug) {
                var count = subscribers.Count;
                string msg = $"New Subscription. Total subscribers: {count}";
                UGX.Debug.Log(msg, gameObject);
            }
        }

        public void Unsubscribe(Func<Event, bool> handler) {
            if (!subscribers.Contains(handler)) return;

            subscribers.Remove(handler);
            if (debug) {
                var count = subscribers.Count;
                string msg = $"Subscription removed. Total subscribers: {count}";
                UGX.Debug.Log(msg, gameObject);
            }
        }

        /// <summary>
        /// Notifies the subscribers about this event and returns
        /// if subscribers want this event to continue propagating
        /// in the scene hierarchy.
        /// </summary>
        /// <param name="ugxEvent">The event instance</param>
        /// <returns>Whether the event should continue propagating</returns>
        public bool Notify(Event ugxEvent) {
            if (debug) {
                string msg = $"Received event: {ugxEvent}";
                UGX.Debug.Log(msg, gameObject);
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
