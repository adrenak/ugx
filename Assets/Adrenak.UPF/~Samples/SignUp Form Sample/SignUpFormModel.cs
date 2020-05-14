using System;
using UnityEngine;

namespace Adrenak.UPF.Examples {
    [Serializable]    
    public class SignUpFormModel : FormModel {
        [SerializeField] string email;
        
        public string Email {
            get => email;
            set => Set(ref email, value);
        }

        [SerializeField] string password;
        
        public string Password {
            get => password;
            set => Set(ref password, value);
        }

        [SerializeField] bool agreeWithTnc;        
        public bool AgreeWithTNC{
            get => agreeWithTnc;
            set => Set(ref agreeWithTnc, value);
        }

        public bool IsValid(){
            if (!AgreeWithTNC)
                return false;

            if (!email.Contains("@"))
                return false;

            if (password.Length < 8)
                return false;
            
            return true;
        }
    }
}
