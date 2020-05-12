using UnityEngine;


namespace Adrenak.UPF.Examples {
    
    public class GreetPage : Page {
        [SerializeField] MarketPage market;

        protected override void InitializePage() { }

        public void GoToMarket() {
            Navigator.PushAsync(market);
        }

        public override void OnAppearing() {
            Debug.Log("GreetPage appearing");
        }

        public override void OnDisappearing() {
            Debug.Log("GreetPage disappaearing");
        }
    }
}
