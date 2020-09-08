using Adrenak.Unex;
using System;

namespace Adrenak.UPF {
    [Serializable]
    public class DefaultNavigationStack : NavigationStack {
        public override void Push(Page page) {
            // First push
            if (History.Count == 0) {
                History.Add(page);
                SetAsCurrent(page);
                return;
            }

            // Repeat push
            if (History.Last().gameObject == page.gameObject)
                return;

            // Alternate repeat push
            if (History.Count > 1 && History.FromLast(1).gameObject == page.gameObject) {
                History.RemoveAt(History.Count - 1);
                SetAsCurrent(History.Last());
                return;
            }

            // All other cases
            History.Add(page);
            SetAsCurrent(page);
        }

        public override void Pop() {
            if (History.Count > 1) {
                History.RemoveAt(History.Count - 1);
                SetAsCurrent(History.Last());
            }
        }

        void SetAsCurrent(Page page) {
            page.OpenPage();
            current?.ClosePage();
            current = page;
        }
    }
}