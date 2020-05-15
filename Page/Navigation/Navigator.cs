using System.Threading.Tasks;
using UnityEngine;
using NaughtyAttributes;

namespace Adrenak.UPF {
    public class Navigator : BindableBehaviour {
#pragma warning disable 0649
        [SerializeField] bool useRootPage;
        [ShowIf("useRootPage")] [SerializeField] PageView rootPage;
        public PageView Root => rootPage;

        [SerializeField] NavigationStack stack = new NavigationStack();
        public NavigationStack Stack => stack;
#pragma warning restore 0649        

        void Start() {
            if (rootPage && useRootPage)
                Push(rootPage);
        }

        public void Push(PageView page) {
            stack.Push(page);
        }

        public void Pop() {
            stack.Pop();
        }
    }
}
