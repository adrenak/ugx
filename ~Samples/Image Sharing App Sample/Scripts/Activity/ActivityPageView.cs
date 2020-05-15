using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples.Unigram{
    public class ActivityPageView : PageView<ActivityPageModel> {
#pragma warning disable 0649
        [SerializeField] ActivityLayoutView layoutView;
        [SerializeField] Text titleDisplay;
#pragma warning restore 0649

        protected override void OnInitializePage() {
            titleDisplay.text = Model.Title;
            layoutView.Items = Model.Notifications;
        }

        protected override void OnPageModelPropertyChanged(string propertyName) {
            OnInitializePage();
        }
    }
}
