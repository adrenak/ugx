using System;
using UnityEngine;

namespace Adrenak.UPF.Examples {
    [Serializable]    
    public class SignUpForm : Form {
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
    }
}
