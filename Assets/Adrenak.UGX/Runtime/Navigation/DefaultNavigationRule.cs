using System.Collections.Generic;

using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// A simple navigation rule that keeps track of history. When a window
    /// already existing in the history is reopened, any entries after it 
    /// are forgotten.
    /// </summary>
    public class DefaultNavigationRule : INavigationRule {
        [SerializeField] List<int> history;

        public string Title => "Default Navigation Rule";

        public void Push(int window) {
            if (history.Contains(window)) {
                var index = history.IndexOf(window);
                history = history.GetRange(0, index + 1);
            }
            else
                history.Add(window);
        }

        public int? Pop() {
            if (history.Count > 1) {
                var toReturn = history[history.Count - 1 - 1];
                history.Remove(toReturn);
                return toReturn;
            }
            else
                return null;
        }
    }
}
