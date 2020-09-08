using UnityEngine;
using Adrenak.Unex;
using UnityEngine.Events;

namespace Adrenak.UPF {
using NaughtyAttributes;
    [DisallowMultipleComponent]
    public class Window : UIBehaviour {
        public UnityEvent onWindowOpen;
        public UnityEvent onWindowClose;
        public UnityEvent onWindowBack;

        public Navigator navigator;
        [ReadOnly] [SerializeField] bool iswindowOpen;
        public bool IsWindowOpen => iswindowOpen;

        bool isOpening, isClosing;

        protected void Update() {
            CheckBackPress();
        }

        void CheckBackPress() {
            if (Input.GetKeyUp(KeyCode.Escape) && IsWindowOpen) {
                navigator.Pop();
                onWindowBack?.Invoke();
                WindowBackPressed();
            }
        }

        [Button]
        public void OpenWindow() {
            if (iswindowOpen || isOpening) return;

            isOpening = true;
            Dispatcher.Enqueue(() => {
                iswindowOpen = true;
                isOpening = false;
            });

            navigator.Push(this);
            WindowOpened();
            onWindowOpen?.Invoke();
        }

        [Button]
        public void CloseWindow() {
            if (!iswindowOpen || isClosing) return;

            isClosing = true;
            Dispatcher.Enqueue(() => {
                iswindowOpen = false;
                isClosing = false;
            });

            navigator.Pop();
            WindowClosed();
            onWindowClose?.Invoke();
        }

        protected virtual void WindowOpened() { }
        protected virtual void WindowClosed() { }
        protected virtual void WindowBackPressed() { }
    }
}