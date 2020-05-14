using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Adrenak.UPF {
    [Serializable]
    public class NavigationStack : Bindable {
#pragma warning disable 0649
        [SerializeField] List<Page> pages;
        public List<Page> Pages => pages;
#pragma warning restore 0649

        public Page Top {
            get {
                if (pages.Count > 0)
                    return pages[pages.Count - 1];
                return null;
            }
        }

        public int Count => pages.Count;

        public void Push(Page page) {
            if (Top != null)
                Top.DisappearPage();
            page.AppearPage();
            pages.Add(page);
        }

        public Page Pop() {
            if (Count > 1) {
                var top = Top;
                top.DisappearPage();
                pages.RemoveAt(pages.Count - 1);
                Top.AppearPage();
                return top;
            }
            return null;
        }
    }
}
