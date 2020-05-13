using UnityEngine;

namespace Adrenak.UPF.Examples {
    public class GreetPage : Page {
#pragma warning disable 0649
        [SerializeField] MarketPage market;
#pragma warning restore 0649

        protected override void OnInitializePage() { }

        public void GoToMarket() {
            Navigator.PushAsync(market);
        }

        override protected void OnAppear() {
            Debug.Log("GreetPage appearing");
        }

        override protected void OnDisappear() {
            Debug.Log("GreetPage disappaearing");
        }
    }
}
