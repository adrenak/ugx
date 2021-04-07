using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Adrenak.UGX {
    /// <summary>
    /// A modified Slider that adds events for when the sliding begins and end.
    /// Useful for making seekbar in video players, hence named so.
    /// </summary>
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
