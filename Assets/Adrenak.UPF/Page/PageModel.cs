﻿using UnityEngine;

namespace Adrenak.UPF.Examples.Unigram {
    [System.Serializable]
    public class PageModel : Model {
        [SerializeField] string title;
        public string Title {
            get => title;
            set => Set(ref title, value);
        }
    }
}