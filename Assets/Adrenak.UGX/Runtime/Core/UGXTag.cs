using System.Collections.Generic;

using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// A tagging utility that allows global access to any of its instances in scene
    /// using a string tag. Much faster than <see cref="GameObject.FindWithTag(string)"/>
    /// </summary>
    public class UGXTag : UGXBehaviour {
        /// <summary>
        /// ID that can be used to identify the View
        /// </summary>
        public string id;

        static Dictionary<string, UGXTag> map = new Dictionary<string, UGXTag>();

        void Awake() => TryRegisterID();

        void OnDestroy() => TryDeregisterID();

        void TryRegisterID() {
            if (string.IsNullOrEmpty(id)) return;
            if (map.ContainsKey(id)) {
                Debug.LogWarning($"Duplicate UGXTag with ID {id}. Instance not registered.", gameObject);
                return;
            }
            map.Add(id, this);
        }

        void TryDeregisterID() {
            if (string.IsNullOrEmpty(id)) return;
            if (!map.ContainsKey(id)) return;
            map.Remove(id);
        }

        /// <summary>
        /// Returns a <see cref="UGXTag"/> instance with 
        /// the given ID
        /// </summary>
        public static UGXTag GetByID(string id) {
            if (map.ContainsKey(id))
                return map[id];
            return null;
        }

        /// <summary>
        /// Returns a <see cref="UGXTag"/> below this instance in 
        /// the scene heirarchy that has the given ID.
        /// </summary>
        public static UGXTag operator / (UGXTag me, string childID) {
            var views = me.GetComponentsInChildren<UGXTag>();
            foreach (var view in views)
                if (view.id.Equals(childID))
                    return view;
            return null;
        }
    }
}
