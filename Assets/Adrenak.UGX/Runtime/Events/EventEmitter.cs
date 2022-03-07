namespace Adrenak.UGX {
    /// <summary>
    /// Used to fire events for consumption by <see cref="EventListener"/>
    /// Currently supports sending events up the scene hierarchy.
    /// </summary>
    public sealed class EventEmitter : UGXBehaviour {
        /// <summary>
        /// Sends a <see cref="Event"/> up the scene hierarchy
        /// </summary>
        /// <param name="id">ID of the event</param>
        public void EmitUpwards(string id) {
            var e = new Event { ID = id, Sender = gameObject };
            EmitUpwards(e);
        }

        /// <summary>
        /// Sends a <see cref="Event"/> up the scene hierarchy with 
        /// an object payload
        /// </summary>
        /// <param name="id">ID of the event</param>
        /// <param name="data">Data associated with the event</param>
        public void EmitUpwards(string id, object data) {
            var ugxEvent = new Event {
                ID = id,
                Data = new object[] { data },
                Sender = gameObject
            };
            EmitUpwards(ugxEvent);
        }

        /// <summary>
        /// Sends an <see cref="Event"/> up the scene heirarchy
        /// with several object payloads
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void EmitUpwards(string id, params object[] data) {
            var ugxEvent = new Event {
                ID = id,
                Data = data,
                Sender = gameObject
            };
            EmitUpwards(ugxEvent);
        }

        void EmitUpwards(Event e) {
            var current = transform;

            while (current != null) {
                var listener = current.GetComponent<EventListener>();
                if (listener != null) {
                    if (!listener.Notify(e))
                        return;
                }
                current = current.parent;
            }
        }
    }
}