using System.Collections.Generic;
using UnityEngine;

namespace Adrenak.UPF.Examples {
    public class OptionPopupSample : MonoBehaviour {
        public OptionPopupView popupView;
        public List<string> options;
        public string title;

        private void Start() {
            popupView.gameObject.GetComponent<UITweener>().FadeInAndForget();

            popupView.Context = new OptionPopupVM() {
                Label = title,
                Options = options
            };

            popupView.Context.OnOptionSelected += (sender, option) =>
                Debug.Log(option);
        }

    }
}
