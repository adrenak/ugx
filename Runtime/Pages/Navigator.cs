using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

namespace Adrenak.UPF {
    public class Navigator : BindableBehaviour {
#pragma warning disable 0649
        public UnityEvent onPush;
        public UnityEvent onPop;

        [SerializeField] bool useRootPage;
        [ShowIf("useRootPage")] [SerializeField] View rootPage;
        public View Root => rootPage;

        [SerializeField] INavigationStack stack = new DefaultNavigationStack();
        public INavigationStack Stack => stack;
#pragma warning restore 0649        

        void Awake() {
            if (rootPage && useRootPage)
                Push(rootPage);
        }

        public void Push(View page) {
            stack.Push(page);
            onPush?.Invoke();
        }

        public void Pop() {
            stack.Pop();
            onPop?.Invoke();
        }
    }
}
