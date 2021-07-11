using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Adrenak.UGX {
    abstract public class Navigator : MonoBehaviour {
        [Serializable]
        public class WindowEvent : UnityEvent<Window> { }

        public WindowEvent onOpen;
        public WindowEvent onBack;
        public UnityEvent onOver;

        [SerializeField] Window currentWindow;
        public Window CurrentWindow => currentWindow;

        [SerializeField] protected List<Window> windows;

        public void RegisterWindow(Window window) {
            window.onWindowStartOpening.AddListener(() => {
                if (currentWindow != null)
                    currentWindow.CloseWindow();

                currentWindow = window;
                onOpen?.Invoke(window);

                OnWindowOpen(window);
            });
            windows.Add(window);
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape))
                Back();
        }

        public void Back() {
            var toOpen = OnPressBack();
            if (toOpen != null) {
                toOpen.OpenWindow();
                onBack?.Invoke(toOpen);
            }
            else
                onOver?.Invoke();
        }

        protected abstract void OnWindowOpen(Window window);
        protected abstract Window OnPressBack();
    }
}
