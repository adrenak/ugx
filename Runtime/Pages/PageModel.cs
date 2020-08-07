using UnityEngine;

namespace Adrenak.UPF {
    [System.Serializable]
    public class PageModel : ViewModel {
        [SerializeField] string pageTitle = string.Empty;
        public string PageTitle {
            get => pageTitle;
            set => Set(ref pageTitle, value);
        }
    }
}
