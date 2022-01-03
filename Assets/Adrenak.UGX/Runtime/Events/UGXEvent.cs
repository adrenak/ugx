using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// Represents an event "packet" that can traverse the scene hierarchy
    /// after being dispatched by <see cref="UGXEventTrigger"/> and listened
    /// for by <see cref="UGXEventListener"/>
    /// </summary>
    public class UGXEvent {
        /// <summary>
        /// A unique ID associated with this event
        /// </summary>
        public string id;

        /// <summary>
        /// The origin of this event, i.e. the <see cref="UGXEventTrigger"/>
        /// that first dispatched this event
        /// </summary>
        public GameObject sender;

        /// <summary>
        /// Any data meant to be sent as payload with this event
        /// </summary>
        public object data;

        public override string ToString() {
            return $"ID : {id}. " +
            $"Sender: {sender.name} ({sender.GetInstanceID()}). " +
            $"Has data {data != null}";
        }
    }
}