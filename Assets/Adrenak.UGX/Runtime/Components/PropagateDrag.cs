using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Adrenak.UGX {
    [RequireComponent(typeof(EventTrigger))]
    public class PropagateDrag : MonoBehaviour {
        void Awake() {
            var scrollViews = GetComponentsInParent<ScrollRect>();
            scrollViews = scrollViews.Where(x => x.gameObject != gameObject).ToArray();

            EventTrigger trigger = GetComponent<EventTrigger>();

            var map = new Dictionary<
                EventTrigger.Entry,
                EventTrigger.TriggerEvent
            >();

            foreach (var t in trigger.triggers) {
                if (t.eventID == EventTriggerType.PointerClick)
                    map.Add(t, t.callback);
            }

            EventTrigger.Entry entryBegin = new EventTrigger.Entry(),
            entryDrag = new EventTrigger.Entry(),
            entryEnd = new EventTrigger.Entry(),
            entrypotential = new EventTrigger.Entry(),
            entryScroll = new EventTrigger.Entry();

            entryBegin.eventID = EventTriggerType.BeginDrag;
            entryBegin.callback.AddListener((data) => {
                foreach (var scrollView in scrollViews)
                    scrollView.OnBeginDrag((PointerEventData)data);
                foreach (var single in map)
                    single.Key.callback = null;
            });
            trigger.triggers.Add(entryBegin);

            entryDrag.eventID = EventTriggerType.Drag;
            entryDrag.callback.AddListener((data) => {
                foreach (var scrollView in scrollViews)
                    scrollView.OnDrag((PointerEventData)data);
            });
            trigger.triggers.Add(entryDrag);

            entryEnd.eventID = EventTriggerType.EndDrag;
            entryEnd.callback.AddListener((data) => {
                foreach (var scrollView in scrollViews)
                    scrollView.OnEndDrag((PointerEventData)data);
                foreach (var single in map)
                    single.Key.callback = single.Value;
            });
            trigger.triggers.Add(entryEnd);

            entrypotential.eventID = EventTriggerType.InitializePotentialDrag;
            entrypotential.callback.AddListener((data) => {
                foreach (var scrollView in scrollViews)
                    scrollView.OnInitializePotentialDrag((PointerEventData)data);
            });
            trigger.triggers.Add(entrypotential);

            entryScroll.eventID = EventTriggerType.Scroll;
            entryScroll.callback.AddListener((data) => {
                foreach (var scrollView in scrollViews)
                    scrollView.OnScroll((PointerEventData)data);
            });
            trigger.triggers.Add(entryScroll);
        }
    }
}