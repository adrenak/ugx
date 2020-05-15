using System.Collections;
using UnityEngine;

namespace Adrenak.UPF.Examples{
    public class PopupSample : MonoBehaviour {
        public AlertPopupView popup;

        IEnumerator Start() {
            yield return new WaitForSeconds(2);
            popup.Model = new AlertPopup {
                Header = "Alert!",
                Body = "This is an alert",
                Ack = "OK"
            };
        }
    }
}
