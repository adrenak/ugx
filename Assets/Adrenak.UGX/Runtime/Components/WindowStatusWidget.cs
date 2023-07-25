using UnityEngine;
using UnityEngine.Events;

namespace Adrenak.UGX {
    /// <summary>
    /// Used to activate/deactivate gameobjects based on the state of a window
    /// <see cref="Window"/>. This can be used to create tab bars, for example.
    /// </summary>
    public class WindowStatusWidget : UGXBehaviour {
        public Window window;

        [Header("Status Objects")]
        public GameObject openedObj;
        public GameObject openingObj;
        public GameObject closedObj;
        public GameObject closingObj;

        [Space(10)]
        [Header("Status Events")]
        public UnityEvent onWindowStartOpening;
        public UnityEvent onWindowDoneOpening;
        public UnityEvent onWindowStartClosing;
        public UnityEvent onWindowDoneClosing;

        void UpdateWithStatus() {
            switch (window.Status) {
                case WindowStatus.Opening:
                    Set(openedObj, false);
                    Set(closingObj, false);
                    Set(closedObj, false);
                    Set(openingObj, true);
                    break;
                case WindowStatus.Opened:
                    Set(closingObj, false);
                    Set(closedObj, false);
                    Set(openingObj, false);
                    Set(openedObj, true);
                    break;
                case WindowStatus.Closing:
                    Set(openedObj, false);
                    Set(closedObj, false);
                    Set(openingObj, false);
                    Set(closingObj, true);
                    break;
                case WindowStatus.Closed:
                    Set(openedObj, false);
                    Set(closingObj, false);
                    Set(openingObj, false);
                    Set(closedObj, true);
                    break;
            }
        }

        void Awake() {
            UpdateWithStatus();

            window.WindowStartedOpening.AddListener(() => {
                UpdateWithStatus();
                onWindowStartOpening.Invoke();
            });
            window.WindowDoneOpening.AddListener(() => {
                UpdateWithStatus();
                onWindowDoneOpening.Invoke();
            });
            window.WindowStartedClosing.AddListener(() => {
                UpdateWithStatus();
                onWindowStartClosing.Invoke();
            });
            window.WindowDoneClosing.AddListener(() => {
                UpdateWithStatus();
                onWindowDoneClosing.Invoke();
            });
        }

        void Set(GameObject go, bool state) {
            if (go != null)
                go.SetActive(state);
        }
    }
}
