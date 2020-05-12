using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Adrenak.UPF {
    
    [System.Serializable]
    public class NavigationStack : Bindable {
        [SerializeField] List<Page> pages;
        
        public List<Page> Pages => pages;

        public Page Top {
            get {
                if (pages.Count > 0)
                    return pages[pages.Count - 1];
                return null;
            }
        }

        public int Count => pages.Count;

        public void Push(Page page) {
            pages.Add(page);
        }

        public Page Pop() {
            var top = Top;
            pages.RemoveAt(pages.Count - 1);
            return top;
        }
    }
}
