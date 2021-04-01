using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Adrenak.UGX {
    public class PopupUsage{
        async void Test(){
            PopupController.Init();
            var alert = new AlertPopup();
            alert.CurrentState = new AlertPopupState {
                header = "Test",
                description = "Test",
                ack = "OK"
            };

            PopupController.Show(alert);

            var response = await alert.WaitForResponse();
        }
    }

    public class PopupController : MonoBehaviour {
        public static void Init() {

        }

        public static void Show<T, K>(Popup<T, K> popup) where T : PopupState where K : PopupResponse {
            
        }
    }
}
