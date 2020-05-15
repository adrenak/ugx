using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples.Unigram{
    public class ActivityPageView : PageView<ActivityPageModel> {
        [SerializeField] ActivityListView listView;
        [SerializeField] Text titleDisplay;

        protected override void OnInitializePage() {
            titleDisplay.text = Model.Title;
            listView.Items = Model.Notifications;
        }

        protected override void OnPageModelPropertyChanged(string propertyName) {
            OnInitializePage();
        }
    }
}
