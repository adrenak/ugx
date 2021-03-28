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

        [ReadOnly] [SerializeField] bool isWindowOpen;
        public bool IsWindowOpen => isWindowOpen;

        public Navigator navigator;
        public bool autoPopOnBack;
        
        [BoxGroup("Orientation")] [SerializeField] bool changeOrientation;
        [BoxGroup("Orientation")] [ShowIf("changeOrientation")] public ScreenOrientation orientation = ScreenOrientation.AutoRotation;

        [BoxGroup("Fullscreen")] [SerializeField] bool changeFullscreen;
        [BoxGroup("Fullscreen")] [ShowIf("changeFullscreen")] public bool isFullscreen = false;

        protected void Update() {
            CheckBackPress();
        }

        void CheckBackPress() {
            if (Input.GetKeyUp(KeyCode.Escape) && IsWindowOpen && autoPopOnBack)
                GoBack();
        }

        [Button]
        public void GoBack() {
            navigator?.Pop();
            onWindowBack?.Invoke();
            WindowBackPressed();
        }

        bool isOpening;
        [Button]
        public void OpenWindow() {
            if (isWindowOpen || isOpening) return;

            if (changeOrientation && Screen.orientation != orientation)
                Screen.orientation = orientation;

            if(changeFullscreen && Screen.fullScreen != isFullscreen)
                Screen.fullScreen = isFullscreen;

            isOpening = true;
            Dispatcher.Enqueue(() => {
                isWindowOpen = true;
                isOpening = false;
            });

            navigator?.Push(this);
            WindowOpened();
            onWindowOpen?.Invoke();
        }

        bool isClosing;
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