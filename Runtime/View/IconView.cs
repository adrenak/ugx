using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Adrenak.UGX {
    [Serializable]
    public class IconViewModel : ViewModel {
        public ViewEvent click;
        
        public string text;
        public string imageURL;
    }

    public class IconView : View<IconViewModel>, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
#pragma warning disable 0649
        [SerializeField] UnityEvent onPointerEnter;
        [SerializeField] UnityEvent onPointerExit;
        [SerializeField] UnityEvent onPointerClick;
        [SerializeField] Text text;
        [SerializeField] Picture picture;
#pragma warning disable 0649

        protected override void OnViewModelSet() {
            gameObject.name = ViewModel.text;
            text.text = ViewModel.text;
            picture.source = Picture.Source.URL;
            picture.path = ViewModel.imageURL;
            if (picture.CurrentVisibility != Visibility.None)
                picture.Refresh();
        }

        public void Click() => ViewModel.click.Invoke(this);
        
        public void OnPointerExit(PointerEventData eventData) {
            onPointerExit.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            onPointerEnter.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData) {
            onPointerClick.Invoke();
        }
    }
}
