using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples {
    public class SignUpFormView : FormView<SignUpFormVM> {
#pragma warning disable 0649
        [SerializeField] InputField emailInput;
        [SerializeField] InputField passwordInput;
        [SerializeField] Toggle tncToggle;
#pragma warning restore 0649

        protected override void InitializeFromContext() {
            emailInput.text = Context.Email;
            passwordInput.text = Context.Password;
            tncToggle.isOn = Context.AgreeWithTNC;
        }

        protected override void OnPropertyChange(string propertyName) {
            switch (propertyName) {
                case nameof(Context.Email):
                    emailInput.text = Context.Email;
                    break;
                case nameof(Context.Password):
                    passwordInput.text = Context.Password;
                    break;
                case nameof(Context.AgreeWithTNC):
                    tncToggle.isOn = Context.AgreeWithTNC;
                    break;
            }
        }

        protected override void BindViewToContext() {
            emailInput.onValueChanged.AddListener(value => Context.Email = value);
            passwordInput.onValueChanged.AddListener(value => Context.Password = value);
            tncToggle.onValueChanged.AddListener(value => Context.AgreeWithTNC = value);
        }
    }
}
