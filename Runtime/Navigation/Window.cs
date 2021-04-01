using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
using NaughtyAttributes;
    [DisallowMultipleComponent]
    public class Window : UGXBehaviour {
        public enum State {
            Closed,
            Closing,
            Opened,
            Opening
        }

        [SerializeField] [ReadOnly] State state;
        public State CurrentState => state;
        [SerializeField] [ReorderableList] TransitionerBase[] transitioners = null;
        [SerializeField] bool showEvents;
        [ShowIf("showEvents")] public UnityEvent onWindowOpen;
        [ShowIf("showEvents")] public UnityEvent onWindowClose;
        [ShowIf("showEvents")] public UnityEvent onWindowBack;

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
            if (Input.GetKeyUp(KeyCode.Escape) && state == State.Opened && autoPopOnBack)
                GoBack();
        }

        [Button]
        async public void OpenWindow() => await OpenWindowAsync();

        async public UniTask OpenWindowAsync() {
            if (state == State.Opened || state == State.Opening) return;

            if (changeOrientation && Screen.orientation != orientation)
                Screen.orientation = orientation;

            if(changeFullscreen && Screen.fullScreen != isFullscreen)
                Screen.fullScreen = isFullscreen;

            state = State.Opening;

            // Wait for all transitioners to transition in
            var transitions = transitioners.Where(x => x.enabled)
                .Select(x => x.TransitionInAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            state = State.Opened;

            WindowOpened();
            onWindowOpen?.Invoke();
        }

        [Button]
        async public void CloseWindow() => await CloseWindowAsync();

        async public UniTask CloseWindowAsync() {
            if (state == State.Closed || state == State.Closing) return;

            state = State.Closing;
            // Wait for all transitioners to transition out
            var transitions = transitioners.Where(x => x.enabled)
                .Select(x => x.TransitionOutAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            state = State.Closed;
            
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