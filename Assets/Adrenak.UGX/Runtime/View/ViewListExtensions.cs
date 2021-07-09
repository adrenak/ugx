using System;
using System.Collections.Generic;

using UnityEngine;

namespace Adrenak.UGX {
    public static class ViewListExtensions {
        class Entry {
            public Type type;
            public object viewList;
        }
        static Dictionary<Transform, List<Entry>> map = new Dictionary<Transform, List<Entry>>();

        public static ViewList<T> ViewList<T>(this Transform container) where T : ViewModel {
            if (map.ContainsKey(container)) {
                var entries = map[container];
                foreach (var entry in entries)
                    if (entry.type == typeof(T))
                        return entry.viewList as ViewList<T>;

                var newViewList = new ViewList<T>(container);
                var newEntry = new Entry {
                    type = typeof(T),
                    viewList = newViewList
                };
                map[container].Add(newEntry);
                return newViewList;
            }
            else {
                var newViewList = new ViewList<T>(container);
                var newEntry = new Entry {
                    type = typeof(T),
                    viewList = newViewList
                };
                map.Add(container, new List<Entry> { newEntry });
                return newViewList;
            }
        }
    }
}