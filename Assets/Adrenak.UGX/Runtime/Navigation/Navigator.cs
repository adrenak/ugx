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
        /// Whether the navigator should pop a window on escape button press
        /// </summary>
        [Tooltip("Whether the navigator should pop on escape button press")]
        public bool popOnEscape = true;

        /// <summary>
        /// Currently opened window
        /// </summary>
        [Tooltip("Currently opened window")]
        [SerializeField] Window currentWindow;

        /// <summary>
        /// Currently open window
        /// </summary>
        public Window CurrentWindow => currentWindow;

        /// <summary>
        /// Windows that this navigator can operate on
        /// </summary>
        [Tooltip("Windows that this navigator can operate on")]
        [SerializeField] List<Window> windows;

        INavigationRule navigationRule;
        /// <summary>
        /// The navigation style in use by this navigator. Can only set this
        /// once.
        /// </summary>
        INavigationRule NavigationRule {
            get => navigationRule;
            set {
                if (navigationRule == null)
                    navigationRule = value;
                else
                    Debug.LogWarning($"Navigation already set to " +
                        $"{navigationRule.Title}", gameObject);
            }
        }

        /// <summary>
        /// Refer to <see cref="Window.Awake"/> to see when this is called.
        /// Although this method is called from another class, this has not
        /// been made public to prevent misuse. 
        /// Instead <see cref="GameObject.SendMessage(string, object)"/> has
        /// been used to invoke it.
        /// </summary>
        void RegisterWindow(object obj) {
            var window = obj as Window;
            window.WindowStartedOpening.AddListener(() => {
                if (currentWindow != null)
                    currentWindow.CloseWindow();

                currentWindow = window;
                WindowPushed.Invoke(window);

                NavigationRule.Push(window.GetInstanceID());
            });
            windows.Add(window);
        }

        /// <summary>
        /// Pop the currently opened window (if any)
        /// </summary>
        public void Back() {
            var toOpen = GetWindowByInstanceID(navigationRule.Pop().Value);
            if (toOpen != null) {
                toOpen.OpenWindow();
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
