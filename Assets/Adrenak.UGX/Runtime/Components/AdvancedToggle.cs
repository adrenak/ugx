using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UGX {
    public class AdvancedToggle : Toggle {
        public Text label;
        public string textOn;
        public string textOff;
        public GameObject objOn;
        public GameObject objOff;

        static readonly ToggleEvent emptyToggleEvent = new ToggleEvent();

        public new void SetIsOnWithoutNotify(bool value) {
            var originalEvent = onValueChanged;
            onValueChanged = emptyToggleEvent;
            isOn = value;
            onValueChanged = originalEvent;

            RefreshLabel();
        }

        new void Awake() {
            base.Awake();

            onValueChanged.AddListener(value => {
                RefreshLabel();
            });

            RefreshLabel();
        }

        void RefreshLabel() {
            if (label != null) {
                if (isOn)
                    label.text = textOn;
                else
                    label.text = textOff;
            }

            if (objOn != null)
                objOn.SetActive(isOn);

            if (objOff != null)
                objOff.SetActive(!isOn);

        }
    }
}
