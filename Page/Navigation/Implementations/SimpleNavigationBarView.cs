using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public class SimpleNavigationBarView : NavigationBarView {
        [SerializeField] Text headerDisplay;
        public string Header {
            get => headerDisplay != null ? headerDisplay.text : string.Empty;
            set { if (headerDisplay != null) headerDisplay.text = value; }
        }
    }
}
