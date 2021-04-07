using System;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;
using UnityEngine.Events;

namespace Adrenak.UGX {
    public class TabBarView : View {
        [Serializable]
        public class Entry {
            public Window window;
            public UnityEvent onOpen;
            public UnityEvent onClose;
        }

#pragma warning disable 0649
        [ReorderableList] [SerializeField] List<Entry> entries;
#pragma warning restore 0649

        void Awake() {
            foreach (var entry in entries) {
                entry.window.onWindowOpen.AddListener(() =>
                    entry.onOpen?.Invoke());

                entry.window.onWindowClose.AddListener(() =>
                    entry.onClose?.Invoke());

                entry.onClose?.Invoke();
            }
        }
    }
}
