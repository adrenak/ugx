using System.Collections.Generic;

using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// Represents an event "packet" that can traverse the scene hierarchy
    /// after being dispatched by <see cref="UGXEventTrigger"/> and listened
    /// for by <see cref="EventListener"/>
    /// </summary>
    public class Event {
        /// <summary>
        /// A unique ID associated with this event
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Any data meant to be sent as payload with this event
        /// </summary>
        public object[] Data { get; set; }

        /// <summary>
        /// The origin of this event, i.e. the <see cref="UGXEventTrigger"/>
        /// that first dispatched this event
        /// </summary>
        public GameObject Sender { get; set; }

        public override string ToString() {
            return $"ID : {ID}. " +
            $"Sender: {Sender.name} ({Sender.GetInstanceID()}). " +
            $"Has data {Data != null}";
        }
    }
}