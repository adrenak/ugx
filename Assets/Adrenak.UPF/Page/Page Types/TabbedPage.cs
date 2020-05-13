using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Adrenak.UPF {
    public class TabbedPage : Page {
        [Header("Tabbed Page")]
        [ReadOnly] [SerializeField] Page current;

        [SerializeField] int startPageIndex;
        
        [ReorderableList] [SerializeField] List<Page> children;
        public List<Page> Children => children;

        void Start() {
            OpenByIndex(startPageIndex);
        }

        public void OpenByIndex(int index) {
            current = Children[index];
            Navigator.PushAsync(current);
        }

        [Button("Auto Populate Pages")]
        public void AutoPopulate() {
            children = GetComponentsInChildren<Page>().ToList()
                .Where(x => x != this)
                .ToList();                
        }
    }
}
