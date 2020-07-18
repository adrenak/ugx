using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using System;

namespace Adrenak.UPF {
    public class TabFlow : MonoBehaviour {
#pragma warning disable 0649
        [Header("Tabbed Page")]
        [ReadOnly] [SerializeField] Page current;

        [SerializeField] int startPageIndex;

        [ReorderableList] [SerializeField] List<Page> children;
        public List<Page> Children => children;
#pragma warning restore 0649

        void Start() {
            OpenByIndex(startPageIndex);
        }

        public void OpenByIndex(int index) {
            if (index < 0 && index >= Children.Count)
                throw new ArgumentException("Index not in range", "index");
            
            current?.ClosePage();
            current = Children[index];
            current.OpenPage();
        }

        [Button("Auto Populate Pages")]
        public void AutoPopulate() {
            children = GetComponentsInChildren<Page>().ToList()
                .Where(x => x != this)
                .ToList();
        }
    }
}
