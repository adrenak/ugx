using System;
using UnityEngine;

namespace Adrenak.UPF.Examples {   
    [Serializable]
    public class ContactModel : Model {
        public event EventHandler OnCall;
        public ref EventHandler onCall => ref OnCall;

        public event EventHandler OnDelete;

#pragma warning disable 0649
        [SerializeField] string _name;       
        public string Name {
            get => _name;
            set => Set(ref _name, value);
        }

        [SerializeField] string _status;
        public string Status {
            get => _status;
            set => Set(ref _status, value);
        }
#pragma warning restore 0649
        
        public void Call() {
            OnCall?.Invoke(this, EventArgs.Empty);
        }

        public void Delete() {
            OnDelete?.Invoke(this, EventArgs.Empty);
        }
    }
}
