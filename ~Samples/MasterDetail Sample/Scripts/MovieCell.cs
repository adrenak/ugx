using System;
using UnityEngine;


namespace Adrenak.UPF.Examples {
    [Serializable]
    
    public class MovieCell : ViewModel {
        public event EventHandler OnClick;

        [SerializeField] string name;
        
        public string Name {
            get => name;
            set => Set(ref name, value);
        }

        [SerializeField] float rating;
        
        public float Rating {
            get => rating;
            set => Set(ref rating, value);
        }

        public void Click() {
            OnClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
