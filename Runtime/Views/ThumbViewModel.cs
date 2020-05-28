using System;
using UnityEngine;

namespace Adrenak.UPF {
    [Serializable]
    public class ThumbViewModel : ViewModel {
        public event EventHandler OnClick;
        public void Click() {
            OnClick?.Invoke(this, null);
        }

        [SerializeField] string text;
        public string Text {
            get => text;
            set => Set(ref text, value);
        }

        [SerializeField] Sprite sprite;
        public Sprite Sprite {
            get => sprite;
            set => Set(ref sprite, value);
        }
    }
}