using System;
using UnityEngine;

namespace Adrenak.UPF.Examples.Unigram {
    [Serializable]
    public class PostPreviewModel : Model {
        [SerializeField] Sprite picture;
        public Sprite Picture {
            get => picture;
            set => Set(ref picture, value);
        }
    }
}
