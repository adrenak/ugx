using System;
using System.Collections.Generic;

using UnityEngine;

namespace Adrenak.UGX {
    public sealed class UGXEventListener : MonoBehaviour {
        Dictionary<string, Func<GameObject, object, bool>> handlers
            = new Dictionary<string, Func<GameObject, object, bool>>();

        public void OnUGXEventWithID(
            string id,
            Func<GameObject, object, bool> handler
        ) => handlers.SetPair(id, handler);

        public bool ProcessUGXEvent(UGXEvent ugxEvent) {
            if (handlers.ContainsKey(ugxEvent.id))
                return handlers[ugxEvent.id](ugxEvent.sender, ugxEvent.data);
            return true;
        }
    }
}
