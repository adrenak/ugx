using System.Threading.Tasks;
using UnityEngine;
using UnityWeld.Binding;

namespace Adrenak.UPF {
    [Binding]
    public class Navigator : BindableBehaviour {
        [SerializeField] NavigationStack stack;
        [Binding]
        public NavigationStack Stack => stack;

        [SerializeField] ContentPage rootPage;
        [Binding]
        public ContentPage Root => rootPage;

        void Start() {
            PushAsync(rootPage);
        }

        async public Task PushAsync(ContentPage page) {
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
