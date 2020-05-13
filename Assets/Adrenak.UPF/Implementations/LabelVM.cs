using UnityEngine;

namespace Adrenak.UPF.Implementations {
    public class LabelVM : ViewModel {
        [SerializeField] string label;
        public string Label {
            get => label;
            set => Set(ref label, value);
        }
    }
}
