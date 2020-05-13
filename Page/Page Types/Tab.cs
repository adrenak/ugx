using System;
using UnityEngine;

namespace Adrenak.UPF {
    public class Tab : View {
        public event EventHandler OnClick;

#pragma warning disable 0649
        [SerializeField] int index;
        public int Index => index;
#pragma warning restore 0649

        public void Click() {
            OnClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
