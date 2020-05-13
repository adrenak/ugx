using UnityEngine;

namespace Adrenak.UPF.Examples {
    public class GreetPage : Page {
#pragma warning disable 0649
        [SerializeField] MarketPage market;
#pragma warning restore 0649

        protected override void InitializePage() { }

        async public void GoToMarket() {
            await Navigator.PushAsync(market);
        }

        public override void OnAppearing() {
            Debug.Log("GreetPage appearing");
        }

        public override void OnDisappearing() {
            Debug.Log("GreetPage disappaearing");
        }
    }
}
