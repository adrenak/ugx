using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Adrenak.UGX {
    [Serializable]
    public class IconState : ViewState {
        public string text;
        public Picture.Source source;
        public string spriteImageURL;
        public string spriteResourcePath;
        public Sprite spriteAsset;
    }

    public class Icon : View<IconState>, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
        public ViewEvent onClick = new ViewEvent();

#pragma warning disable 0649
        [SerializeField] UnityEvent onPointerEnter;
        [SerializeField] UnityEvent onPointerExit;
        [SerializeField] UnityEvent onPointerClick;
        [SerializeField] Text text;
        [SerializeField] Picture picture;
#pragma warning disable 0649

        protected override void HandleViewStateSet() {
            gameObject.name = CurrentState.text;
            text.text = CurrentState.text;
            picture.source = CurrentState.source;

            switch (CurrentState.source) {
                case Picture.Source.Asset:
                    picture.sprite = CurrentState.spriteAsset;
                    break;
                case Picture.Source.Resource:
                    picture.path = CurrentState.spriteResourcePath;
                    break;
                case Picture.Source.URL:
                    picture.path = CurrentState.spriteImageURL;
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
