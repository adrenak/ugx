using UnityEngine;

namespace Adrenak.UGX {
    public sealed class UGXEventTrigger : MonoBehaviour {
        public bool debug;

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
                var listener = loc.GetComponent<UGXEventListener>();
                if (listener != null) {
                    if (!listener.SendUGXEvent(ugxEvent))
                        return;
                }
                loc = loc.parent;
            }
        }
    }
}