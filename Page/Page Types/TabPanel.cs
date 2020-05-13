using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Adrenak.UPF {
    public class TabPanel : View {
#pragma warning disable 0649
        [SerializeField] TabbedPage page;
        public TabbedPage Page => page;

        [SerializeField] List<Tab> tabs;
        public List<Tab> Tabs => tabs;
#pragma warning restore 0649

        void Start() {
            foreach (var tab in tabs) {
                tab.OnClick += async (sender, args) =>
                    await Page.OpenByIndex(tab.Index);
            }
        }

        [ContextMenu("Auto Populate Pages")]
        public void AutoPopulate() {
            tabs = GetComponentsInChildren<Tab>().ToList();
        }
    }
}
