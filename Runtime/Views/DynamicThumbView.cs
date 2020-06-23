using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Adrenak.UPF {
    public class DynamicThumbView : View<DynamicThumbViewModel>, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
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

        protected override void InitializeView() {
            Refresh();
        }

        protected override void Refresh() {
            text.text = ViewModel.Text;
            image.source = DynamicImage.Source.RemotePath;
            image.path = ViewModel.ImageURL;
        }

        protected override void ObserveModel(string propertyName) {
            Refresh();
        }

        protected override void ObserveView() { }

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
