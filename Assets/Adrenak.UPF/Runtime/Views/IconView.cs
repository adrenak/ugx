using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public class IconView : View<IconViewModel>, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

#pragma warning disable 0649
        [SerializeField] UnityEvent onPointerEnter = null;
        [SerializeField] UnityEvent onPointerExit = null;
        [SerializeField] UnityEvent onPointerClick;
        [SerializeField] Text text;
        [SerializeField] Image image;
#pragma warning disable 0649

        protected override void OnViewModelSet() {
            text.text = ViewModel.text;
            image.sprite = ViewModel.sprite;
        }

        public void OnPointerClick(PointerEventData eventData) {
            onPointerClick.Invoke();
            ViewModel.Click();
        }

        public void OnPointerExit(PointerEventData eventData) {
            onPointerExit.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            onPointerEnter.Invoke();
        }
    }
}
