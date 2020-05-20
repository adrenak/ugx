using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples.Unigram{
    public class PostPreviewView : View<PostPreviewModel> {
        [SerializeField] Image pictureDisplay = null;

        protected override void OnSetModel() {
            pictureDisplay.sprite = Model.Picture;
        }

        protected override void ObserveModel(string propertyName) {
            OnSetModel();
        }
 
        protected override void ObserveView() { }
    }
}
