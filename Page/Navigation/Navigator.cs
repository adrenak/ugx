using System.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UPF {
    public class Navigator : BindableBehaviour {
#pragma warning disable 0649
        [SerializeField] NavigationStack stack;
        public NavigationStack Stack => stack;

        [SerializeField] Page rootPage;
        public Page Root => rootPage;
#pragma warning restore 0649        

        void Start() {
            PushAsync(rootPage);
        }

        async public Task PushAsync(Page page) {
            if(stack.Top != null)
                stack.Top.OnDisappearing();
            page.OnAppearing();
            stack.Push(page);
        }

        async public Task PopAsync() {
            if(stack.Count > 1){
                stack.Pop().OnDisappearing();
                stack.Top.OnAppearing();                
            }
        }
    }
}
