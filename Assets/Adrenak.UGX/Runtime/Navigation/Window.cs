using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
using NaughtyAttributes;
    [DisallowMultipleComponent]
    public class Window : UGXBehaviour {
        [SerializeField] [ReorderableList] TransitionerBase[] transitioners;
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
        async public void OpenWindow() => await OpenWindowAsync();

        bool isOpening;
        async public UniTask OpenWindowAsync() {
            if (isWindowOpen || isOpening) return;

            if (changeOrientation && Screen.orientation != orientation)
                Screen.orientation = orientation;

            if(changeFullscreen && Screen.fullScreen != isFullscreen)
                Screen.fullScreen = isFullscreen;

            isOpening = true;

            // Wait for all transitioners to transition in
            var transitions = transitioners.Where(x => x.enabled)
                .Select(x => x.TransitionInAsync())
                .ToList();

            await UniTask.WhenAll(transitions);

            await UniTask.SwitchToMainThread();
            isWindowOpen = true;
            isOpening = false;

            WindowOpened();
            onWindowOpen?.Invoke();
        }

        [Button]
        async public void CloseWindow() => await CloseWindowAsync();

        bool isClosing;
        async public UniTask CloseWindowAsync() {
            if (!isWindowOpen || isClosing) return;

            isClosing = true;
            // Wait for all transitioners to transition out
            var transitions = transitioners.Where(x => x.enabled)
                .Select(x => x.TransitionOutAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            isWindowOpen = false;
            isClosing = false;
            
            WindowClosed();
            onWindowClose?.Invoke();
        }

        [Button]
        public void GoBack() {
            WindowBackPressed();
            onWindowBack?.Invoke();
        }

        protected virtual void WindowOpened() { }
        protected virtual void WindowClosed() { }
        protected virtual void WindowBackPressed() { }
    }
}