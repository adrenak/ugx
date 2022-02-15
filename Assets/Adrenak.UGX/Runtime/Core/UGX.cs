using System;
using System.Collections.Generic;

namespace Adrenak.UGX {
    public static partial class UGX {
        public static event Action<Event> EventReceived;

        public static void SetState(string key, object value) {
            if (State.ContainsKey(key))
                State[key].Value = value;
            else
                State.Add(key, new Observable(value));
        }

        public static object GetState(string key) {
            if (!State.ContainsKey(key)) return null;

            return State[key].Value;
        }

        public static object GetStateObservable(string key) {
            if (!State.ContainsKey(key)) return null;
            return State[key];
        }

        static Dictionary<string, Observable> State
            = new Dictionary<string, Observable>();

        public static void BroadcastEvent(Event e) {
            EventReceived?.Invoke(e);
        }
    }
}
