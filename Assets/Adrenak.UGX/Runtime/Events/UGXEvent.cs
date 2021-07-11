using UnityEngine;

namespace Adrenak.UGX {
    public class UGXEvent {
        public string id;
        public GameObject sender;
        public object data;

        public override string ToString() {
            return $"ID : {id}. " +
            $"Sender: {sender.name} ({sender.GetInstanceID()}). " +
            $"Has data {data != null}";
        }
    }
}