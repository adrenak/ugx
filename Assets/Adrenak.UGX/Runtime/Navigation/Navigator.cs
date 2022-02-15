using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Adrenak.UGX {
    /// <summary>
    /// Manages opening and closing of multiple <see cref="Window"/> using a 
    /// <see cref="INavigationRule"/>
    /// </summary>
    public sealed class Navigator : MonoBehaviour {
        [Serializable] public class WindowPushEvent : UnityEvent<Window> { }
        [Serializable] public class WindowPopEvent : UnityEvent<Window> { }

        /// <summary>
        /// Invoked when a new window is pushed into the navigator
        /// </summary>
        public WindowPushEvent WindowPushed = new WindowPushEvent();

        /// <summary>
        /// Invoked when a window is popped from the navigator
        /// </summary>
        public WindowPopEvent WindowPopped = new WindowPopEvent();

        /// <summary>
        /// Invoked when popping leads to no window left to open
        /// </summary>
        public UnityEvent WindowsOver;

        /// <summary>
        /// Whether the <see cref="Window"/> instances open and close
        /// in a sequential manner. If false, a window will start opening
        /// at the same at time one starts closing. If true, the 
        /// opening window will wait for the closing window to finish
        /// closing before it starts to open.
        /// </summary>
        public bool sequential = false;

        /// <summary>
        /// Whether the navigator should pop the active window on 
        /// escape button press
        /// </summary>
        [Tooltip("Whether the navigator should pop on escape button press")]
        public bool popOnEscape = true;

        /// <summary>
        /// Currently opened window
        /// </summary>
        public Window CurrentWindow => router.ActiveWindow;

        /// <summary>
        /// Windows that this navigator can operate on
        /// </summary>
        [Tooltip("Windows that this navigator can operate on")]
        [SerializeField] List<Window> windows;

        [SerializeField] Router router;

        INavigationRule navigationRule;
        /// <summary>
        /// The navigation style in use by this navigator. Can only set this
        /// once.
        /// </summary>
        public INavigationRule NavigationRule {
            get => navigationRule;
            set {
                if (navigationRule == null)
                    navigationRule = value;
                else
                    UGX.Debug.LogWarning($"Navigation already set to " +
                        $"{navigationRule.GetType().FullName}", gameObject);
            }
        }

        /// <summary>
        /// Refer to <see cref="Window.Awake"/> to see when this is called.
        /// Although this method is called from another class, this has not
        /// been made public to prevent misuse. 
        /// Instead <see cref="GameObject.SendMessage(string, object)"/> has
        /// been used to invoke it.
        /// </summary>
        public void RegisterWindow(object obj) {
            var window = obj as Window;
            window.WindowStartedOpening.AddListener(() => {
                router.SetActiveWindow(window);;
                WindowPushed.Invoke(window);
                NavigationRule.Push(window.GetInstanceID());
            });
            windows.Add(window);
        }

        /// <summary>
        /// Pop the currently opened window (if any)
        /// </summary>
        public void Back() {
            var toOpen = GetWindowByInstanceID(NavigationRule.Pop().Value);
            if (toOpen != null) {
                router.SetActiveWindow(toOpen);
                WindowPopped.Invoke(toOpen);
            }
            else
                WindowsOver.Invoke();
        }

        void Update() {
            if (popOnEscape && Input.GetKeyDown(KeyCode.Escape))
                Back();
        }

        Window GetWindowByInstanceID(int id) {
            foreach (var window in windows)
                if (window.GetInstanceID() == id)
                    return window;
            return null;
        }
    }
}
