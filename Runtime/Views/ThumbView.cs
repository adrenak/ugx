using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public class ThumbView : View<ThumbModel> {
#pragma warning disable 0649
        [SerializeField] Text text;
        [SerializeField] Image image;
#pragma warning disable 0649

        public void Click() {
            Model.Click();
        }

        protected override void InitializeView() {
            Refresh();
        }

        protected override void Refresh() {
            text.text = Model.Text;
            image.sprite = Model.Sprite;
        }

        protected override void ObserveModel(string propertyName) {
            Refresh();
        }

        protected override void ObserveView() { }
    }
}
