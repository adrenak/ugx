using System;
using UnityEngine;
using System.Collections.Generic;

namespace Adrenak.UPF{
    [Serializable]
    public class OptionPopupVM : Model {
        public event EventHandler<string> OnOptionSelected;

        [SerializeField] string label;
        public string Label {
            get => label;
            set => Set(ref label, value);
        }

        [SerializeField] List<string> options = new List<string>();
        public List<string> Options {
            get => options;
            set => Set(ref options, value);
        }

        public void Select(string option) {
            OnOptionSelected?.Invoke(this, option);
        }
    }
}
