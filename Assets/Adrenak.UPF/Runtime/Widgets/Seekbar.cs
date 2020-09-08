using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public class Seekbar : Slider {
        public SliderEvent onBeginSeek;
        public SliderEvent onEndSeek;

        public override void OnPointerDown(PointerEventData eventData) {
            base.OnPointerDown(eventData);
            onBeginSeek?.Invoke(value);
        }

        public override void OnPointerUp(PointerEventData eventData) {
            base.OnPointerUp(eventData);
            onEndSeek?.Invoke(value);
        }
    }
}
