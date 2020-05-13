using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Implementations {
    public class LabelListItem : ViewModel {
        [SerializeField] string label;
        public string Label {
            get => label;
            set => Set(ref label, value);
        }
    }

    public class LabelListItemView : View<LabelListItem> {
#pragma warning disable 0649
        [SerializeField] Text labelDisplay;
#pragma warning restore 0649
        
        protected override void BindViewToContext() { }

        protected override void InitializeFromContext() {
            labelDisplay.text = Context.Label;
        }

        protected override void OnPropertyChange(string propertyName) {
            if (propertyName.Equals(nameof(Context.Label)))
                labelDisplay.text = Context.Label;
        }
    }
}
