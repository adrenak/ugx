using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Adrenak.UGX {
    public class NestedScrollRect : ScrollRect {
        //The parent BaseScrollRect object
        private ScrollRect m_Parent;

        public enum Direction {
            Horizontal,
            Vertical
        }
        //Slide direction
        private Direction m_Direction = Direction.Horizontal;
        //Current operation direction
        private Direction m_BeginDragDirection = Direction.Horizontal;

        protected override void Awake() {
            base.Awake();
            //Find the parent object
            Transform parent = transform.parent;
            if (parent)
                m_Parent = parent.GetComponentInParent<ScrollRect>();
            m_Direction = this.horizontal ? Direction.Horizontal : Direction.Vertical;
        }

        public override void OnBeginDrag(PointerEventData eventData) {
            if (m_Parent) {
                m_BeginDragDirection = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y) ? Direction.Horizontal : Direction.Vertical;
                if (m_BeginDragDirection != m_Direction) {
                    ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
                    //The current operation direction is not equal to the sliding direction, and the event is passed to the parent object
                    return;
                }
            }

            base.OnBeginDrag(eventData);
        }
        public override void OnDrag(PointerEventData eventData) {
            if (m_Parent) {
                if (m_BeginDragDirection != m_Direction) {
                    ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.dragHandler);
                    //The current operation direction is not equal to the sliding direction, and the event is passed to the parent object
                    return;
                }
            }
            base.OnDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData) {
            if (m_Parent) {
                if (m_BeginDragDirection != m_Direction) {
                    ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.endDragHandler);
                    //The current operation direction is not equal to the sliding direction, and the event is passed to the parent object
                    return;
                }
            }
            base.OnEndDrag(eventData);
        }

        public override void OnScroll(PointerEventData data) {
            if (m_Parent) {
                if (m_BeginDragDirection != m_Direction) {
                    ExecuteEvents.Execute(m_Parent.gameObject, data, ExecuteEvents.scrollHandler);
                    //The current operation direction is not equal to the sliding direction, and the event is passed to the parent object
                    return;
                }
            }
            base.OnScroll(data);
        }
    }
}