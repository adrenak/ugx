using System;
using UnityEngine;

namespace Adrenak.UPF.Examples.Unigram {
    [Serializable]
    public class ProfileSummaryModel : Model {
        public event EventHandler OnOpenWebsite;
        public event EventHandler OnEditProfile;

        [SerializeField] string username;
        public string Username {
            get => username;
            set => Set(ref username, value);
        }

        [SerializeField] string displayName;
        public string DisplayName {
            get => displayName;
            set => Set(ref displayName, value);
        }

        [SerializeField] Sprite displayPicture;
        public Sprite DisplayPicture {
            get => displayPicture;
            set => Set(ref displayPicture, value);
        }

        [SerializeField] string bio;
        public string Bio {
            get => bio;
            set => Set(ref bio, value);
        }

        [SerializeField] string website;
        public string Website {
            get => website;
            set => Set(ref website, value);
        }

        [SerializeField] int postCount;
        public int PostCount {
            get => postCount;
            set => Set(ref postCount, value);
        }

        [SerializeField] int followerCount;
        public int FollowerCount {
            get => followerCount;
            set => Set(ref followerCount, value);
        }

        [SerializeField] int followingCount;
        public int FollowingCount {
            get => followingCount;
            set => Set(ref followingCount, value);
        }

        public void OpenWebsite() {
            OnOpenWebsite?.Invoke(this, EventArgs.Empty);
        }

        public void EditProfile() {
            OnEditProfile?.Invoke(this, EventArgs.Empty);
        }
    }
}
