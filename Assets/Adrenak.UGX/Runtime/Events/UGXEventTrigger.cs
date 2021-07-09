using UnityEngine;

namespace Adrenak.UGX {
    public sealed class UGXEventTrigger : MonoBehaviour {
        public void SendUGXEventUpwards(string id) {
            var ugxEvent = new UGXEvent { id = id, sender = gameObject };
            SendUGXEventUpwards(ugxEvent);
        }

        public void SendUGXEventUpwards(string id, object data) {
            var ugxEvent = new UGXEvent {
                id = id,
                data = data,
                sender = gameObject
            };
            SendUGXEventUpwards(ugxEvent);
        }

        void SendUGXEventUpwards(UGXEvent ugxEvent) {
            Transform loc = transform;
            while (loc != null) {
                var listeners = loc.GetComponentsInParent<UGXEventListener>();
                if (listeners != null && listeners.Length > 0) {
                    bool propagate = true;
                    foreach (var listener in listeners) {
                        // If any listener on this location wants to 
                        // stop the propagation of event, detect it.
                        if (!listener.ProcessUGXEvent(ugxEvent))
                            propagate = false;
                    }
                    if (!propagate) return;
                }
                loc = loc.parent;
            }
        }
    }
}
