using UnityEngine;
using UnityWeld.Binding;

namespace Adrenak.UPF.Examples {
    [Binding]
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
