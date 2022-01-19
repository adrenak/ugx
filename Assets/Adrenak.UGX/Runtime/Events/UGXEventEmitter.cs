using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// Used to fire events for consumption by <see cref="UGXEventListener"/>
    /// Currently supports sending events up the scene hierarchy.
    /// </summary>
    public sealed class UGXEventEmitter: MonoBehaviour {
        /// <summary>
        /// Sends a <see cref="UGXEvent"/> up the scene hierarchy
        /// </summary>
        /// <param name="id">ID of the event</param>
        public void EmitUpwards(string id) {
            var ugxEvent = new UGXEvent { id = id, sender = gameObject };
            EmitUpwards(ugxEvent);
        }

        /// <summary>
        /// Sends a <see cref="UGXEvent"/> up the scene hierarchy
        /// </summary>
        /// <param name="id">ID of the event</param>
        /// <param name="data">Data associated with the event</param>
        public void EmitUpwards(string id, object data) {
            var ugxEvent = new UGXEvent {
                id = id,
                data = data,
                sender = gameObject
            };
            EmitUpwards(ugxEvent);
        }

        void EmitUpwards(UGXEvent ugxEvent) {
            var current = transform;
            while (current != null) {
                var listener = current.GetComponent<UGXEventListener>();
                if (listener != null) {
                    if (!listener.Notify(ugxEvent))
                        return;
                }
                current = current.parent;
            }
        }
    }
}