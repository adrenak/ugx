using System.Threading.Tasks;
using UnityEngine;
using NaughtyAttributes;

namespace Adrenak.UPF {
    public class Navigator : BindableBehaviour {
#pragma warning disable 0649
        [SerializeField] bool useRootPage;
        [ShowIf("useRootPage")] [SerializeField] Page rootPage;
        public Page Root => rootPage;

        [SerializeField] NavigationStack stack;
        public NavigationStack Stack => stack;
#pragma warning restore 0649        

        void Start() {
            if(rootPage)
                PushAsync(rootPage);
        }

        public void PushAsync(Page page) {
            if(stack.Top != null)
                stack.Top.Disappear();
            page.Appear();
            stack.Push(page);
        }

        public void PopAsync() {
            if(stack.Count > 1){
                stack.Pop().Disappear();
                stack.Top.Appear();                
            }
        }
    }
}
