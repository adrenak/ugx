using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UPF {
    public class TabbedPage : Page {
        [SerializeField] List<Page> children;
        public List<Page> Children => children;

        async public Task OpenByIndex(int index) {
            await Navigator.PushAsync(Children[index]);
        }

        [ContextMenu("Auto Populate Pages")]
        public void AutoPopulate() {
            children = GetComponentsInChildren<Page>().ToList()
                .Where(x => x != this)
                .ToList();                
        }
    }
}
