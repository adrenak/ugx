using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Adrenak.UPF {
    public class DIconView : View<DIconViewModel>, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
#pragma warning disable 0649
        [SerializeField] UnityEvent onPointerEnter;
        [SerializeField] UnityEvent onPointerExit;
        [SerializeField] UnityEvent onPointerClick;
        [SerializeField] Text text;
        [SerializeField] DynamicImage image;
#pragma warning disable 0649

        public void Click() {
            ViewModel.Click();
        }

        protected override void OnViewModelSet() {
            gameObject.name = ViewModel.text;
            text.text = ViewModel.text;
            image.source = DynamicImage.Source.URL;
            image.path = ViewModel.imageURL;
            if (image.CurrentVisibility != Visibility.None)
                image.Refresh();
        }
        
        public void OnPointerExit(PointerEventData eventData) {
            onPointerExit.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            onPointerEnter.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData) {
            onPointerClick.Invoke();
            ViewModel?.Click();
        }
    }
}
