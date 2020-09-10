using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Adrenak.UGX {
    [Serializable]
    public class IconViewModel : ViewModel {
        public Picture.Source source;       
        public string text;
        public string imageURL;
        public string resourcePath;
        public Sprite sprite;
    }

    public class IconView : View<IconViewModel>, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
        public ViewEvent onClick = new ViewEvent();

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
            picture.source = ViewModel.source;

            switch (ViewModel.source) {
                case Picture.Source.Asset:
                    picture.sprite = ViewModel.sprite;
                    break;
                case Picture.Source.Resource:
                    picture.path = ViewModel.resourcePath;
                    break;
                case Picture.Source.URL:
                    picture.path = ViewModel.imageURL;
                    break;
            }

            if (picture.CurrentVisibility != Visibility.None)
                picture.Refresh();
        }

        public void Click() =>
            onClick.Invoke(this);        
        
        public void OnPointerExit(PointerEventData eventData) =>
            onPointerExit.Invoke();

        public void OnPointerEnter(PointerEventData eventData) =>
            onPointerEnter.Invoke();

        public void OnPointerClick(PointerEventData eventData) =>
            onPointerClick.Invoke();
    }
}
