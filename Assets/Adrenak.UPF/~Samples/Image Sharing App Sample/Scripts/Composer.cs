using UnityEngine;

namespace Adrenak.UPF.Examples.Unigram{
    public class Composer : MonoBehaviour {
        public ActivityPageView activityPage;
        public ActivityPageModel activityModel;

        public ProfilePageView profilePage;
        public ProfilePageModel profileModel;

        public void Start() {
            activityPage.Model = activityModel;
            profilePage.Model = profileModel;

            profilePage.Model.ProfileSummary.OnOpenWebsite += (sender, e)
                => Application.OpenURL(profileModel.ProfileSummary.Website);
        }
    }
}
