using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples.Unigram{
    public class PostPreviewView : View<PostPreviewModel> {
        [SerializeField] Image pictureDisplay = null;

        protected override void Refresh() {
            pictureDisplay.sprite = Model.Picture;
        }

        protected override void ObserveModel(string propertyName) {
            Refresh();
        }
 
        protected override void ObserveView() { }
    }
}
