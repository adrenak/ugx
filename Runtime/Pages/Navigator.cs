using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Adrenak.UPF {
    public class Navigator : MonoBehaviour {
#pragma warning disable 0649
        public UnityEvent onPush;
        public UnityEvent onPop;

        [SerializeField] [ReadOnly] List<View> _history = new List<View>();
        [SerializeField] NavigationStack stack = new DefaultNavigationStack();

        [SerializeField] bool useRootPage;
        [ShowIf("useRootPage")] [SerializeField] View rootPage;
#pragma warning restore 0649        

        public List<View> History => _history;
        public View Root => rootPage;

        void Awake() {
            if (rootPage && useRootPage)
                Push(rootPage);
        }

        public void Push(View view) {
            stack.Push(view);
            onPush?.Invoke();
        }

        public void Pop() {
            stack.Pop();
            onPop?.Invoke();
        }
    }
}
