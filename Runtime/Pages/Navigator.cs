using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

namespace Adrenak.UPF {
    public class Navigator : MonoBehaviour {
#pragma warning disable 0649
        public UnityEvent onPush;
        public UnityEvent onPop;

        [SerializeField] NavigationStack stack = new DefaultNavigationStack();
        public NavigationStack Stack { get => stack; }

        [SerializeField] bool useRootPage;
        [ShowIf("useRootPage")] [SerializeField] View rootPage;
#pragma warning restore 0649

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
