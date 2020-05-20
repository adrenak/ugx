using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples.Unigram {
    public class ProfileSummaryView : View<ProfileSummaryModel> {
#pragma warning disable 0649
        [SerializeField] Text summaryUsernameDisplay;
        [SerializeField] Text displayNameDisplay;
        [SerializeField] Image displayPictureDisplay;
        [SerializeField] Text bioDisplay;
        [SerializeField] Text websiteDisplay;
        [SerializeField] Text postCountDisplay;
        [SerializeField] Text followerCountDisplay;
        [SerializeField] Text followingCountDisplay;
#pragma warning restore 0649

        protected override void OnSetModel() {
            summaryUsernameDisplay.text = Model.Username;
            displayNameDisplay.text = Model.DisplayName;
            displayPictureDisplay.sprite = Model.DisplayPicture;
            bioDisplay.text = Model.Bio;
            websiteDisplay.text = Model.Website;
            postCountDisplay.text = Model.PostCount.ToString();
            followerCountDisplay.text = Model.FollowerCount.ToString();
            followingCountDisplay.text = Model.FollowingCount.ToString();
        }

        public void OpenWebsite() {
            Model.OpenWebsite();
        }

        public void EditProfile() {
            Model.EditProfile();
        }

        protected override void ObserveView() { }
        protected override void ObserveModel(string propertyName) {
            OnSetModel();
        }
    }
}
