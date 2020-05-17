using System.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UPF.Examples{
    public class PopupSample : MonoBehaviour {
        public AlertPopupView popup;
        public ConfirmationPopupView confirmation;

        async void Start() {
            await Task.Delay(1000);

            await popup.Show("Someheader", "Body goes here", "OK");
            Debug.Log("Responded to alert view!");
            
            await Task.Delay(1000);

            var response = await confirmation.Show("Question", "Are you sure?", "Yes", "No");
            Debug.Log("Responded to confirmation view with " + response);
        }
    }
}
