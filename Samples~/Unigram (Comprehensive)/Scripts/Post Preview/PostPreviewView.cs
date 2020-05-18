using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples.Unigram{
    public class PostPreviewView : View<PostPreviewModel> {
        [SerializeField] Image pictureDisplay = null;

        protected override void OnSetViewModel() {
            pictureDisplay.sprite = Model.Picture;
        }

        protected override void OnViewModelPropertyChanged(string propertyName) {
            OnSetViewModel();
        }
 
        protected override void OnObserveView() { }
    }
}
