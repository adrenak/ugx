using System;
using UnityEngine;


namespace Adrenak.UPF.Examples {
    
    [Serializable]
    public class ContactCellViewModel : ViewModel {
        public event EventHandler OnCall;
        public event EventHandler OnClick;
        public event EventHandler OnDelete;

        [SerializeField] string _name;
        
        public string Name {
            get => _name;
            set => Set(ref _name, value);
        }

        public ContactCellViewModel() { }

        [SerializeField] string _status;
        
        public string Status {
            get => _status;
            set => Set(ref _status, value);
        }

        public void Call() {
            OnCall?.Invoke(this, EventArgs.Empty);
        }

        public void Delete() {
            OnDelete?.Invoke(this, EventArgs.Empty);
        }

        public void Click(){
            OnClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
