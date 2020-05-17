using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples {
    public class SignUpFormView : FormView<SignUpFormModel> {
#pragma warning disable 0649
        [SerializeField] InputField emailInput;
        [SerializeField] InputField passwordInput;
        [SerializeField] Toggle tncToggle;
#pragma warning restore 0649

        protected override void OnSetViewModel() {
            emailInput.text = Model.Email;
            passwordInput.text = Model.Password;
            tncToggle.isOn = Model.AgreeWithTNC;
        }

        protected override void OnViewModelPropertyChanged(string propertyName) {
            switch (propertyName) {
                case nameof(Model.Email):
                    emailInput.text = Model.Email;
                    break;
                case nameof(Model.Password):
                    passwordInput.text = Model.Password;
                    break;
                case nameof(Model.AgreeWithTNC):
                    tncToggle.isOn = Model.AgreeWithTNC;
                    break;
            }
        }

        protected override void OnObserveView() {
            emailInput.onValueChanged.AddListener(value => Model.Email = value);
            passwordInput.onValueChanged.AddListener(value => Model.Password = value);
            tncToggle.onValueChanged.AddListener(value => Model.AgreeWithTNC = value);
        }
    }
}
