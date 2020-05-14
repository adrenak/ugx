using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples.Unigram{
    public class PostPreviewView : View<PostPreviewModel> {
        [SerializeField] Image pictureDisplay = null;

        protected override void InitializeView() {
            pictureDisplay.sprite = Model.Picture;
        }

        protected override void OnModelPropertyChanged(string propertyName) {
            InitializeView();
        }
 
        protected override void ListenToView() { }
    }
}
