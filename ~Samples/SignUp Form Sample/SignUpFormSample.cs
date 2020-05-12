using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples {
    public class SignUpFormSample : MonoBehaviour {
        public SignUpFormView form;
        public Text message;

        void Start() {
            form.Context.OnSubmit += (sender, args) => {
                if (!form.Context.AgreeWithTNC)
                    message.text = "Please read to T&C";
                else
                    message.text = $"Send a sign up request for credentials {form.Context.Email} + {form.Context.Password}";
            };

            form.Context.OnCancel += (sender, args) =>
                message.text = "User cancelled sign up. Maybe first ask if they're sure?";
        }
    }
}
