using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Adrenak.UGX {
    [Serializable]
    public class IconViewState : ViewState {
        public string text;
        public Picture.Source source;
        public string spriteImageURL;
        public string spriteResourcePath;
        public Sprite spriteAsset;
    }

    public class IconView : StatefulView<IconViewState>, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
        public UnityEvent onClick = new UnityEvent();

#pragma warning disable 0649
        [SerializeField] UnityEvent onPointerEnter;
        [SerializeField] UnityEvent onPointerExit;
        [SerializeField] UnityEvent onPointerClick;
        [SerializeField] Text text;
        [SerializeField] Picture picture;
#pragma warning disable 0649

        protected override void HandleStateSet() {
            text.text = State.text;
            picture.source = State.source;

            switch (State.source) {
                case Picture.Source.Asset:
                    picture.sprite = State.spriteAsset;
                    break;
                case Picture.Source.Resource:
                    picture.path = State.spriteResourcePath;
                    break;
                case Picture.Source.URL:
                    picture.path = State.spriteImageURL;
                    break;
            }

            picture.Refresh();
        }

        public void Click() =>
            onClick?.Invoke();        
        
        public void OnPointerExit(PointerEventData eventData) =>
            onPointerExit.Invoke();

        public void OnPointerEnter(PointerEventData eventData) =>
            onPointerEnter.Invoke();

        public void OnPointerClick(PointerEventData eventData) =>
            onPointerClick.Invoke();
    }
}
