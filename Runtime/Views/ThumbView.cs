using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public class ThumbView : View<ThumbViewModel>, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] UnityEvent onPointerEnter = null;
        [SerializeField] UnityEvent onPointerExit = null;

#pragma warning disable 0649
        [SerializeField] Text text;
        [SerializeField] Image image;
#pragma warning disable 0649

        protected override void InitializeView() {
            HandleViewModelSet();
        }

        protected override void HandleViewModelSet() {
            text.text = ViewModel.Text;
            image.sprite = ViewModel.Sprite;
        }

        protected override void HandleViewModelMoficiation(string propertyName) {
            HandleViewModelSet();
        }

        protected override void ObserveView() { }

        public void OnPointerClick(PointerEventData eventData) {
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
