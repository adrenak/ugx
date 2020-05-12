using System;
using UnityEngine;
using UnityWeld.Binding;

namespace Adrenak.UPF.Examples {
    [Binding]
    [Serializable]
    public class ContactCellViewModel : ViewModel {
        public event EventHandler OnCall;
        public event EventHandler OnClick;
        public event EventHandler OnDelete;

        [SerializeField] string _name;
        [Binding]
        public string Name {
            get => _name;
            set => Set(ref _name, value);
        }

        [SerializeField] string _status;
        [Binding]
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
