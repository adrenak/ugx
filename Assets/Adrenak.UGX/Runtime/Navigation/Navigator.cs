using System;
using System.Collections.Generic;

using UnityEngine;

namespace Adrenak.UGX {
    abstract public class Navigator : MonoBehaviour {
        public event Action<Window> OnOpen;
        public event Action<Window> OnBack;
        public event Action OnOver;

        [SerializeField] Window currentWindow;
        public Window CurrentWindow => currentWindow;

        [SerializeField] protected List<Window> windows;

        public void RegisterWindow(Window window) {
            window.onWindowStartOpening.AddListener(() => {
                if (currentWindow != null)
                    currentWindow.CloseWindow();

                currentWindow = window;
                OnOpen?.Invoke(window);

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
                OnBack?.Invoke(toOpen);
            }
            else
                OnOver?.Invoke();
        }

        protected abstract void OnWindowOpen(Window window);
        protected abstract Window OnPressBack();
    }
}
