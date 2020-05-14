using UnityEngine;

namespace Adrenak.UPF.Examples.Unigram{
    public class ProfileView : View<ProfileModel> {
        [SerializeField] ProfileSummaryView summaryView;
        [SerializeField] PostPreviewSetView previewSetView;

        protected override void InitializeView() {
            summaryView.Model = Model.ProfileSummary;
            previewSetView.ItemsSource.AddFrom(Model.PostPreviews);
        }

        protected override void OnModelPropertyChanged(string propertyName) {
            InitializeView();
        }

        protected override void ListenToView() { }

        protected override void OnStart() {
            Model.ProfileSummary.OnEditProfile += (sender, e)
                => Debug.Log("Edit Profile");

            Model.ProfileSummary.OnOpenWebsite += (sender, e)
                => Application.OpenURL(Model.ProfileSummary.Website);
        }
    }
}
