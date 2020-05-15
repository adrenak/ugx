using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Adrenak.UPF {
    public class TabFlow : MonoBehaviour {
#pragma warning disable 0649
        [Header("Tabbed Page")]
        [ReadOnly] [SerializeField] PageView current;

        [SerializeField] int startPageIndex;

        [SerializeField] Navigator navigator;
        
        [ReorderableList] [SerializeField] List<PageView> children;
        public List<PageView> Children => children;
#pragma warning restore 0649

        void Start() {
            OpenByIndex(startPageIndex);
        }

        public void OpenByIndex(int index) {
            current = Children[index];
            navigator.Push(current);
        }

        [Button("Auto Populate Pages")]
        public void AutoPopulate() {
            children = GetComponentsInChildren<PageView>().ToList()
                .Where(x => x != this)
                .ToList();
        }
    }
}
