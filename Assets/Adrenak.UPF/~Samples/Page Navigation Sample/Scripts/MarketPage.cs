using UnityEngine;

namespace Adrenak.UPF.Examples {
    public class MarketPage : Page {
#pragma warning disable 0649
        [SerializeField] GreetPage greet;
#pragma warning restore 0649

        async public void GoToGreet(){
            await Navigator.PushAsync(greet);
        }

        public override void OnAppearing() {
            Debug.Log("MarketPage appearing");
        }

        public override void OnDisappearing() {
            Debug.Log("MarketPage disappaearing");
        }
    }
}
