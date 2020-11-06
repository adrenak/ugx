using Adrenak.Unex;

namespace Adrenak.UGX {
    public class DefaultNavigator : Navigator {
        void Start() {
            if (initialWindow && useInitialWindow)
                Push(initialWindow);
        }

        public override void Push(Window window) {
            // First push
            if (History.Count == 0) {
                History.Add(window);
                SetAsCurrent(window);
                return;
            }

            // Repeat push
            if (History.Last().gameObject == window.gameObject)
                return;

            // Alternate repeat push
            if (History.Count > 1 && History.FromLast(1).gameObject == window.gameObject) {
                History.RemoveAt(History.Count - 1);
                SetAsCurrent(History.Last());
                return;
            }

            // All other cases
            History.Add(window);
            SetAsCurrent(window);

            onPush?.Invoke();
        }

        public override void Pop() {
            if (History.Count > 1) {
                History.RemoveAt(History.Count - 1);
                SetAsCurrent(History.Last());
                onPop?.Invoke();
            }
        }

        public override void Clear() {
            while(History.Count != 0)
                Pop();
        }
    }
}
