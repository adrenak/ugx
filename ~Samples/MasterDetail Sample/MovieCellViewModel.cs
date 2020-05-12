using System;
using UnityEngine;
using UnityWeld.Binding;

namespace Adrenak.UPF.Examples {
    [Serializable]
    [Binding]
    public class MovieCellViewModel : ListItemViewModel {
        [SerializeField] string name;
        [Binding]
        public string Name {
            get => name;
            set => Set(ref name, value);
        }

        [SerializeField] float rating;
        [Binding]
        public float Rating {
            get => rating;
            set => Set(ref rating, value);
        }
    }
}
