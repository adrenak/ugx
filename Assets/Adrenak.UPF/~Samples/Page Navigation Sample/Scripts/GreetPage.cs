using UnityEngine;


namespace Adrenak.UPF.Examples {
    
    public class GreetPage : ContentPage {
        [SerializeField] MarketPage market;

        protected override void InitializePage() { }

        public void GoToMarket() {
            Navigation.PushAsync(market);
        }

        public override void OnAppearing() {
            Debug.Log("GreetPage appearing");
        }

        public override void OnDisappearing() {
            Debug.Log("GreetPage disappaearing");
        }
    }
}
