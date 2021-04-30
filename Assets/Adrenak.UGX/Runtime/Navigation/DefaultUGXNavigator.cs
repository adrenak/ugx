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

            onPush.Invoke(window);
        }

        protected override void PopImpl() {
            if (History.Count > 1) {
                var last = History.Last();
                History.RemoveLast();
                SetAsCurrent(History.Last());
                onPop.Invoke(last);
            }
            else if(History.Count == 1){
                History.Last().CloseWindow();
                var first = History[0];
                History.Remove(first);
                current = null;
                onPop.Invoke(first);
            }
        }
    }
}
