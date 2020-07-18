using UnityEngine;

namespace Adrenak.UPF {
    [System.Serializable]
    public class PageModel : ViewModel {
        [SerializeField] string title = string.Empty;
        public string Title {
            get => title;
            set => Set(ref title, value);
        }
    }
}
