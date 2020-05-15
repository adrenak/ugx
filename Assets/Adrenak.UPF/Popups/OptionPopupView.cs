using UnityEngine;
using UnityEngine.UI;
using Adrenak.UPF.Implementations;
using System.Linq;

namespace Adrenak.UPF {
    public class OptionPopupView : View<OptionPopupVM> {
#pragma warning disable 0649
        [SerializeField] Text labelDisplay;
        [SerializeField] LabelListView listView;
#pragma warning restore 0649

        void Start() {
            listView.OnItemSelected += (sender, e) =>
                Model.Select((e.Item as LabelModel).Label);
        }

        protected override void OnViewInitialize() {
            labelDisplay.text = Model.Label;

            listView.Items.Clear();
            listView.Items.AddFrom(
                Model.Options
                .Select(x => new LabelModel { Label = x })
                .ToList()
            );
        }

        protected override void OnObserveViewEvents() { }
        protected override void OnViewModelPropertyChanged(string propertyName) { }
    }
}
