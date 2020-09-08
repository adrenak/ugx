using UnityEngine;
using Adrenak.Unex;
using UnityEngine.Events;

namespace Adrenak.UPF {
using NaughtyAttributes;
    [DisallowMultipleComponent]
    public class Page : UIBehaviour {
        public UnityEvent onPageOpen;
        public UnityEvent onPageClose;
        public UnityEvent onBackPress;

        public Navigator navigator;
        [ReadOnly] [SerializeField] bool isOpen;
        public bool IsOpen => isOpen;

        bool isOpening, isClosing;

        protected void Update() {
            CheckBackPress();
        }

        void CheckBackPress() {
            if (Input.GetKeyUp(KeyCode.Escape) && IsOpen) {
                navigator.Pop();
                onBackPress?.Invoke();
                BackPressed();
            }
        }

        [Button]
        public void OpenPage() {
            if (isOpen || isOpening) return;

            isOpening = true;
            Dispatcher.Enqueue(() => {
                isOpen = true;
                isOpening = false;
            });

            navigator.Push(this);
            PageOpened();
            onPageOpen?.Invoke();
        }

        [Button]
        public void ClosePage() {
            if (!isOpen || isClosing) return;

            isClosing = true;
            Dispatcher.Enqueue(() => {
                isOpen = false;
                isClosing = false;
            });

            navigator.Pop();
            PageClosed();
            onPageClose?.Invoke();
        }

        protected virtual void PageOpened() { }
        protected virtual void PageClosed() { }
        protected virtual void BackPressed() { }
    }
}