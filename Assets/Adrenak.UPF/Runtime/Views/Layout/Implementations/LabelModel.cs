using UnityEngine;

namespace Adrenak.UPF.Implementations {
    public class LabelModel : Model {
        [SerializeField] string label;
        public string Label {
            get => label;
            set => Set(ref label, value);
        }
    }
}
