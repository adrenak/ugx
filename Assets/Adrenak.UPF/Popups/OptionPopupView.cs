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
                Context.Select((e.Item as LabelVM).Label);
        }

        protected override void OnSetContext() {
            labelDisplay.text = Context.Label;

            listView.ItemsSource.Clear();
            listView.ItemsSource.AddFrom(
                Context.Options
                .Select(x => new LabelVM { Label = x })
                .ToList()
            );
        }

        protected override void BindViewToContext() { }
        protected override void OnPropertyChange(string propertyName) { }
    }
}
