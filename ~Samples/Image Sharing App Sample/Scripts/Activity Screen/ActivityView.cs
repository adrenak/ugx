using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples.Unigram {
    public class ActivityView : View<ActivityModel> {
#pragma warning disable 0649
        [SerializeField] Image userDPDisplay;
        [SerializeField] Text usernameDisplay;
        [SerializeField] Text summaryDisplay;
        [SerializeField] Image subjectDPDisplay;
        [SerializeField] Text timeAgoDisplay;
#pragma warning restore 0649

        protected override void OnViewInitialize() {
            userDPDisplay.sprite = Model.UserDP;
            usernameDisplay.text = Model.Username;
            summaryDisplay.text = Model.Summary;
            subjectDPDisplay.sprite = Model.SubjectDP;
            timeAgoDisplay.text = Model.TimeAgo;
        }

        protected override void OnObserveViewEvents() { }

        protected override void OnViewModelPropertyChanged(string propertyName) {
            OnViewInitialize();
        }
    }
}
