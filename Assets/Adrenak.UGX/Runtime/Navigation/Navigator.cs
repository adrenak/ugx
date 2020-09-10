using NaughtyAttributes;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Adrenak.UGX {
    public abstract class Navigator : MonoBehaviour {
#pragma warning disable 0649
        [ReadOnly] [SerializeField] protected Window current = null;
        public Window Current => current;

        [ReadOnly] [ReorderableList] [SerializeField] protected List<Window> history = new List<Window>();
        public List<Window> History => history;

        public UnityEvent onPush;
        public UnityEvent onPop;

        [SerializeField] protected bool useInitialWindow;
        [ShowIf("useInitialWindow")] [SerializeField] protected Window initialWindow;
#pragma warning restore 0649

        public abstract void Push(Window window);
        public abstract void Pop();

        protected void SetAsCurrent(Window window) {
            window.OpenWindow();
            current?.CloseWindow();
            current = window;
        }
    }
}
