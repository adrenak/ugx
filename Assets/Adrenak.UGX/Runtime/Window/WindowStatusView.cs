using UnityEngine;
using UnityEngine.Events;

namespace Adrenak.UGX {
    public class WindowStatusView : View {
        public Window window;
        public GameObject open;
        public GameObject closed;

        [Space(10)]
        public UnityEvent onWindowStartOpening;
        public UnityEvent onWindowStartClosing;

        void Awake() {
            window.onWindowStartOpening.AddListener(() => {
                if (open != null)
                    open.SetActive(true);
                if (closed != null)
                    closed.SetActive(false);
                onWindowStartOpening.Invoke();
            });
            window.onWindowStartClosing.AddListener(() => {
                if (open != null)
                    open.SetActive(false);
                if (closed != null)
                    closed.SetActive(true);
                onWindowStartClosing.Invoke();
            });

            if (open != null) {
                open.SetActive(
                    window.Status == WindowStatus.Opened ||
                    window.Status == WindowStatus.Opening
                );
            }

            if (closed != null) {
                closed.SetActive(
                    window.Status == WindowStatus.Closed ||
                    window.Status == WindowStatus.Closing
                );
            }
        }
    }
}
