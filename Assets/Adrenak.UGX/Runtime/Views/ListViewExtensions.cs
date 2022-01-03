using System;
using System.Collections.Generic;

using UnityEngine;

namespace Adrenak.UGX {
    public static class ListViewExtensions {
        class Entry {
            public Type type;
            public object listView;
        }

        static Dictionary<Transform, List<Entry>> map
            = new Dictionary<Transform, List<Entry>>();

        /// <summary>
        /// Allows creating a <see cref="ListView{T}"/> using a transform as its container 
        /// </summary>
        /// <typeparam name="T">The <see cref="State"/> type</typeparam>
        /// <param name="container">The transform to use</param>
        /// <returns></returns>
        public static ListView<T> ListView<T>(this Transform container) where T : State {
            if (map.ContainsKey(container)) {
                var entries = map[container];
                foreach (var entry in entries)
                    if (entry.type == typeof(T))
                        return entry.listView as ListView<T>;

                var newListView = new ListView<T>(container);
                var newEntry = new Entry {
                    type = typeof(T),
                    listView = newListView
                };
                map[container].Add(newEntry);
                return newListView;
            }
            else {
                var newListView = new ListView<T>(container);
                var newEntry = new Entry {
                    type = typeof(T),
                    listView = newListView
                };
                map.Add(container, new List<Entry> { newEntry });
                return newListView;
            }
        }
    }
}