using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public class TextNavigationBarView : NavigationBarView {
#pragma warning disable 0649
        [SerializeField] Text headerDisplay;
#pragma warning restore 0649

        public string Header {
            get => headerDisplay != null ? headerDisplay.text : string.Empty;
            set { if (headerDisplay != null) headerDisplay.text = value; }
        }
    }
}
