using UnityEngine;
using Adrenak.Unex;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
using NaughtyAttributes;
    [DisallowMultipleComponent]
    public class Window : UGXBehaviour {
        [SerializeField] bool showEvents;
        [ShowIf("showEvents")] public UnityEvent onWindowOpen;
        [ShowIf("showEvents")] public UnityEvent onWindowClose;
        [ShowIf("showEvents")] public UnityEvent onWindowBack;

        [ReadOnly] [SerializeField] bool isWindowOpen;
        public bool IsWindowOpen => isWindowOpen;

        public bool autoPopOnBack;
        
        [BoxGroup("Orientation")] [SerializeField] bool changeOrientation = false;
        [BoxGroup("Orientation")] [ShowIf("changeOrientation")] public ScreenOrientation orientation = ScreenOrientation.AutoRotation;

        [BoxGroup("Fullscreen")] [SerializeField] bool changeFullscreen = false;
        [BoxGroup("Fullscreen")] [ShowIf("changeFullscreen")] public bool isFullscreen = false;

        protected new void Update() {
            base.Update();
            CheckBackPress();
        }

        void CheckBackPress() {
            if (Input.GetKeyUp(KeyCode.Escape) && IsWindowOpen && autoPopOnBack)
                GoBack();
        }

        [Button]
        public void GoBack() {
            WindowBackPressed();
            onWindowBack?.Invoke();
        }

        bool isOpening;
        [Button]
        async public void OpenWindow() {
            if (isWindowOpen || isOpening) return;

            if (changeOrientation && Screen.orientation != orientation)
                Screen.orientation = orientation;

            if(changeFullscreen && Screen.fullScreen != isFullscreen)
                Screen.fullScreen = isFullscreen;

            isOpening = true;
            await UniTask.SwitchToMainThread();
            isWindowOpen = true;
            isOpening = false;

            WindowOpened();
            onWindowOpen?.Invoke();
        }

        bool isClosing;
        [Button]
        async public void CloseWindow() {
            if (!isWindowOpen || isClosing) return;

            isClosing = true;
            await UniTask.SwitchToMainThread();
            isWindowOpen = false;
            isClosing = false;
            
            WindowClosed();
            onWindowClose?.Invoke();
        }

        protected virtual void WindowOpened() { }
        protected virtual void WindowClosed() { }
        protected virtual void WindowBackPressed() { }
    }
}