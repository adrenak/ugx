using System;
using UnityEngine;

namespace Adrenak.UPF {
    [Serializable]
    public class DynamicThumbViewModel : ViewModel {
        public event EventHandler OnClick;
        public void Click() {
            OnClick?.Invoke(this, null);
        }

        [SerializeField] string text;
        public string Text {
            get => text;
            set => Set(ref text, value);
        }

        [SerializeField] string imageURL;
        public string ImageURL {
            get => imageURL;
            set => Set(ref imageURL, value);
        }
    }
}