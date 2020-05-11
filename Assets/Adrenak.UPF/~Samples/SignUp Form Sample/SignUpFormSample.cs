using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples {
    public class SignUpFormSample : MonoBehaviour {
        public SignUpFormView form;
        public Text message;

        void Start() {
            form.BindingContext.OnSubmit += (sender, args) => {
                if (!form.BindingContext.AgreeWithTNC)
                    message.text = "Please read to T&C";
                else
                    message.text = $"Send a sign up request for credentials {form.BindingContext.Email} + {form.BindingContext.Password}";
            };

            form.BindingContext.OnCancel += (sender, args) =>
                message.text = "User cancelled sign up. Maybe first ask if they're sure?";
        }
    }
}
