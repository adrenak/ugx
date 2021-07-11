using UnityEngine;
using UnityEngine.Events;

namespace Adrenak.UGX {
    public class WindowStatusView : View {
        public Window window;

        public GameObject open;
        public GameObject close;

        [Space(10)]
        public UnityEvent onWindowStartOpening;
        public UnityEvent onWindowDoneOpening;
        public UnityEvent onWindowStartClosing;
        public UnityEvent onWindowDoneClosing;

        void UpdateWithStatus() {
            switch (window.Status) {
                case WindowStatus.Opening:
                    Set(true, false);
                    break;
                case WindowStatus.Opened:
                    Set(true, false);
                    break;
                case WindowStatus.Closing:
                    Set(false, true);
                    break;
                case WindowStatus.Closed:
                    Set(false, true);
                    break;
            }
        }

        void Awake() {
            UpdateWithStatus();

            window.onWindowStartOpening.AddListener(() => {
                UpdateWithStatus();
                onWindowStartOpening.Invoke();
            });
            window.onWindowDoneOpening.AddListener(() => {
                UpdateWithStatus();
                onWindowDoneOpening.Invoke();
            });
            window.onWindowStartClosing.AddListener(() => {
                UpdateWithStatus();
                onWindowStartClosing.Invoke();
            });
            window.onWindowDoneClosing.AddListener(() => {
                UpdateWithStatus();
                onWindowDoneClosing.Invoke();
            });
        }

        void Set(bool _open, bool _close) {
            Set(open, _open);
            Set(close, _close);
        }

        void Set(GameObject go, bool state) {
            if (go != null)
                go.SetActive(state);
        }
    }
}
