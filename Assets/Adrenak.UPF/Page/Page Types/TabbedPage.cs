using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UPF {
    public class TabbedPage : Page {
        [SerializeField] Page current;
        [SerializeField] List<Page> children;
        public List<Page> Children => children;

        public void OpenByIndex(int index) {
            current = Children[index];
            Navigator.PushAsync(current);
        }

        [ContextMenu("Auto Populate Pages")]
        public void AutoPopulate() {
            children = GetComponentsInChildren<Page>().ToList()
                .Where(x => x != this)
                .ToList();                
        }
    }
}
