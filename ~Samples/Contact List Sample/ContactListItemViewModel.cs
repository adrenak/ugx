using System;
using UnityEngine;
using UnityWeld.Binding;

namespace Adrenak.UPF.Examples{
    [Binding]
    [Serializable]
    public class ContactListItemViewModel : ViewModel {
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
    }
}
