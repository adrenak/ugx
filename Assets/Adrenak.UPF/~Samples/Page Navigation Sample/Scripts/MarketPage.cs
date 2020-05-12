using UnityEngine;


namespace Adrenak.UPF.Examples {
    
    public class MarketPage : Page {
        [SerializeField] GreetPage greet;

        public void GoToGreet(){
            Navigator.PushAsync(greet);
        }

        public override void OnAppearing() {
            Debug.Log("MarketPage appearing");
        }

        public override void OnDisappearing() {
            Debug.Log("MarketPage disappaearing");
        }
    }
}
