using System.Collections.Generic;

using UnityEngine;

namespace Adrenak.UGX {
    public class NonRepeatingHistoryNavigator : Navigator {
        [SerializeField] List<Window> history;

        override protected void OnWindowOpen(Window window) {
            if (history.Contains(window)) {
                var index = history.IndexOf(window);
                history = history.GetRange(0, index + 1);
            }
            else
                history.Add(window);
        }

        override protected Window OnPressBack() {
            if (history.Count > 1)
                return history[history.Count - 1 - 1];
            else
                return null;
        }
    }
}
