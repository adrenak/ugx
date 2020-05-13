using System;
using UnityEngine;

namespace Adrenak.UPF.Examples {
    [Serializable]   
    public class MovieVM : ViewModel {
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
    }
}
