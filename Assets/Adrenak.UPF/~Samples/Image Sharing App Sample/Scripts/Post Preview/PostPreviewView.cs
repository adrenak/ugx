using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples.Unigram{
    public class PostPreviewView : View<PostPreviewModel> {
        [SerializeField] Image pictureDisplay = null;

        protected override void OnViewInitialize() {
            pictureDisplay.sprite = Model.Picture;
        }

        protected override void OnViewModelPropertyChanged(string propertyName) {
            OnViewInitialize();
        }
 
        protected override void OnObserveViewEvents() { }
    }
}
