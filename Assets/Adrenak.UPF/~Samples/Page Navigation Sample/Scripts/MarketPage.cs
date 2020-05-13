using UnityEngine;

namespace Adrenak.UPF.Examples {
    public class MarketPage : Page {
#pragma warning disable 0649
        [SerializeField] GreetPage greet;
#pragma warning restore 0649

        public void GoToGreet(){
            Navigator.PushAsync(greet);
        }

        override protected void OnAppear() {
            Debug.Log("MarketPage appearing");
        }

        override protected void OnDisappear() {
            Debug.Log("MarketPage disappaearing");
        }
    }
}
