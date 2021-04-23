using Adrenak.Unex;

namespace Adrenak.UGX {
    public class DefaultUGXNavigator : Navigator {
        void Start() {
            if (initialWindow && useInitialWindow)
                PushImpl(initialWindow);
        }

        protected override void PushImpl(Window window) {
            // First push
            if (History.Count == 0) {
                History.Add(window);
                SetAsActive(window);
                return;
            }

            // Repeat push
            if (History.Last().gameObject == window.gameObject)
                return;

            // Alternate repeat push
            if (History.Count > 1 && History.FromLast(1).gameObject == window.gameObject) {
                History.RemoveAt(History.Count - 1);
                SetAsActive(History.Last());
                return;
            }

            // All other cases
            History.Add(window);
            SetAsActive(window);

            onPush?.Invoke();
        }

        protected override void PopImpl() {
            if (History.Count > 0) {
                History.RemoveAt(History.Count - 1);
                SetAsActive(History.Last());
                onPop?.Invoke();
            }
        }
    }
}
