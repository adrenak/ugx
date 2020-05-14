using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples {
    public class SignUpFormSample : MonoBehaviour {
        public SignUpFormView form;
        public Text message;

        void Start() {
            form.Model.OnSubmit += (sender, args) => {
                if (form.Model.IsValid())
                    message.text = $"Send a sign up request for credentials {form.Model.Email} + {form.Model.Password}";
                else
                    message.text = "Password should be at least 8 characters. Email should be valid. Terms and conditions should be accepted";
            };

            form.Model.OnCancel += (sender, args) =>
                message.text = "Cancelling, are you sure?";
        }
    }
}
