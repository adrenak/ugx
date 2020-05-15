using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples.Unigram{
    public class ProfilePageView : PageView<ProfilePageModel> {
#pragma warning disable 0649
        [SerializeField] ProfileSummaryView summaryView;
        [SerializeField] PostPreviewLayoutView previewSetView;
        [SerializeField] Text titleDisplay;
#pragma warning restore 0649
        
        protected override void OnInitializePage() {
            titleDisplay.text = Model.Title;
            summaryView.Model = Model.ProfileSummary;
            previewSetView.Items.AddFrom(Model.PostPreviews);
        }

        protected override void OnPageModelPropertyChanged(string propertyName) {
            OnInitializePage();
        }
    }
}
