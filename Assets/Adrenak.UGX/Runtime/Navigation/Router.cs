using UnityEngine;

namespace Adrenak.UGX {
    [System.Serializable]
    public class Router {
        /// <summary>
        /// A singleton instance. Can be used to manage <see cref="Window"/>
        /// instances when not using a <see cref="Navigator"/>. This class
        /// is also used internally inside <see cref="Navigator"/>
        /// </summary>
        public static Router Global { get; private set; } = new Router();

        [SerializeField] Window activeWindow;
        public Window ActiveWindow {
            get => activeWindow;
            set {
                if (activeWindow != null)
                    activeWindow.CloseWindow();
                activeWindow = value;
                value.OpenWindow();
            }
        }

        public async void SetActiveWindow(Window window, bool async = true) {
            if (activeWindow != null) {
                if (async)
                    await activeWindow.CloseWindowAsync();
                else
                    activeWindow.CloseWindow();
            }
            activeWindow = window;

            if (async)
                await activeWindow.OpenWindowAsync();
            else
                activeWindow.OpenWindow();
        }
    }
}
