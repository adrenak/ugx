using UnityEngine;

namespace Adrenak.UPF.Examples.Unigram {
    [System.Serializable]
    public class NotificationModel : Model {
        [SerializeField] Sprite userDP;
        public Sprite UserDP {
            get => userDP;
            set => Set(ref userDP, value);
        }

        [SerializeField] string username;
        public string Username {
            get => username;
            set => Set(ref username, value);
        }

        [SerializeField] string summary;
        public string Summary {
            get => summary;
            set => Set(ref summary, value);
        }

        [SerializeField] Sprite subjectDP;
        public Sprite SubjectDP {
            get => subjectDP;
            set => Set(ref subjectDP, value);
        }

        [SerializeField] string timeAgo;
        public string TimeAgo {
            get => timeAgo;
            set => Set(ref timeAgo, value);
        }
    }
}
