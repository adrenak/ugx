using System.Collections.Generic;
using UnityEngine;
using Adrenak.Unex;
using NaughtyAttributes;
using System;

namespace Adrenak.UPF {
    [Serializable]
    public class DefaultNavigationStack : INavigationStack {
        [ReadOnly] [SerializeField] View current = null;
        public View Current => current;

        [ReorderableList] [ReadOnly] [SerializeField] List<View> history = new List<View>();
        public List<View> History => history;

        public void Push(View page) {
            // First push
            if (history.Count == 0) {
                history.Add(page);
                SetAsCurrent(page);
                return;
            }

            // Repeat push
            if (history.Last() == page)
                return;

            // Alternate repeat push
            if (history.Count > 1 && history.FromLast(1) == page) {
                history.RemoveAt(history.Count - 1);
                SetAsCurrent(history.Last());
                return;
            }

            // All other cases
            history.Add(page);
            SetAsCurrent(page);
        }

        public void Pop() {
            if (history.Count > 1) {
                history.RemoveAt(history.Count - 1);
                SetAsCurrent(history.Last());
            }
        }

        void SetAsCurrent(View page) {
            page.OpenPage();
            current?.ClosePage();
            current = page;
        }
    }
}