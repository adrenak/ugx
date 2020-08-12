﻿using System;
using UnityEngine;

namespace Adrenak.UPF {
    [Serializable]
    public class IconViewModel : ViewModel {
        public event EventHandler OnClick;
        public void Click() => OnClick?.Invoke(this, null);

        public string text;
        public Sprite sprite;
    }
}