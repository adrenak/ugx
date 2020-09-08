using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

namespace Adrenak.UPF {
    public class Navigator : MonoBehaviour {
#pragma warning disable 0649
        public UnityEvent onPush;
        public UnityEvent onPop;

        [SerializeField] NavigationStack stack;
        public NavigationStack Stack { 
            get => stack;
            set => stack = value;
        }

        [SerializeField] bool useRootPage;
        [ShowIf("useRootPage")] [SerializeField] Window rootPage;
#pragma warning restore 0649

        void Awake() {
            if (rootPage && useRootPage)
                Push(rootPage);
        }

        public void Push(Window view) {
            stack.Push(view);
            onPush?.Invoke();
        }

        public void Pop() {
            stack.Pop();
            onPop?.Invoke();
        }
    }
}
