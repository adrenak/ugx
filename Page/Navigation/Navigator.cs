using System.Threading.Tasks;
using UnityEngine;


namespace Adrenak.UPF {
    
    public class Navigator : BindableBehaviour {
        [SerializeField] NavigationStack stack;
        
        public NavigationStack Stack => stack;

        [SerializeField] Page rootPage;
        
        public Page Root => rootPage;

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
