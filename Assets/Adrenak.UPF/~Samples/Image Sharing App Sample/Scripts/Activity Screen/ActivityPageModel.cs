using System.Collections.Generic;
using UnityEngine;

namespace Adrenak.UPF.Examples.Unigram {
    [System.Serializable]
    public class ActivityPageModel : PageModel {
        [SerializeField] List<ActivityModel> notifications;
        public List<ActivityModel> Notifications {
            get => notifications;
            set => Set(ref notifications, value);
        }
    }
}
