using UnityEngine;
using Adrenak.Unex;
using UnityEngine.Events;

namespace Adrenak.UGX {
using NaughtyAttributes;
    [DisallowMultipleComponent]
    public class Window : UIBehaviour {
        [SerializeField] bool showEvents;
        [ShowIf("showEvents")] public UnityEvent onWindowOpen;
        [ShowIf("showEvents")] public UnityEvent onWindowClose;
        [ShowIf("showEvents")] public UnityEvent onWindowBack;

        public Navigator navigator;
        [ReadOnly] [SerializeField] bool isWindowOpen;
        public bool IsWindowOpen => isWindowOpen;

        bool isOpening, isClosing;

        protected void Update() {
            CheckBackPress();
        }

        void CheckBackPress() {
            if (Input.GetKeyUp(KeyCode.Escape) && IsWindowOpen)
                GoBack();
        }

        [Button]
        public void GoBack() {
            navigator.Pop();
            onWindowBack?.Invoke();
            WindowBackPressed();
        }

        [Button]
        public void OpenWindow() {
            if (isWindowOpen || isOpening) return;

            isOpening = true;
            Dispatcher.Enqueue(() => {
                isWindowOpen = true;
                isOpening = false;
            });

            navigator.Push(this);
            WindowOpened();
            onWindowOpen?.Invoke();
        }

        [Button]
        public void CloseWindow() {
            if (!isWindowOpen || isClosing) return;

            isClosing = true;
            Dispatcher.Enqueue(() => {
                isWindowOpen = false;
                isClosing = false;
            });

            WindowClosed();
            onWindowClose?.Invoke();
        }

        protected virtual void WindowOpened() { }
        protected virtual void WindowClosed() { }
        protected virtual void WindowBackPressed() { }
    }
}