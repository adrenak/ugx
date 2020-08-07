using System;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;
using UnityEngine.Events;

namespace Adrenak.UPF {
    public class TabBarVisualizer : MonoBehaviour {
        [Serializable]
        public class Entry {
            public View page;
            public UnityEvent onOpen;
            public UnityEvent onClose;
        }

#pragma warning disable 0649
        [ReorderableList] [SerializeField] List<Entry> entries;
#pragma warning restore 0649

        void Awake() {
            foreach (var entry in entries) {
                entry.page.onPageOpen.AddListener(() =>
                    entry.onOpen?.Invoke());

                entry.page.onPageClose.AddListener(() =>
                    entry.onClose?.Invoke());
            }
        }
    }
}
