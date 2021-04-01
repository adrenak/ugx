using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;

namespace Adrenak.UGX {
    public enum WindowStatus {
        Closed,
        Closing,
        Opened,
        Opening
    }

    public class Window : Window<WindowState> {
        protected override void HandleWindowStateSet() { }
    }

    [System.Serializable]
    public class WindowState : ViewState {
        public WindowStatus status;
        public bool autoPopOnBack = true;
    }

    public abstract class Window<T> : View<T> where T : WindowState {
        [SerializeField] bool showEvents;
        [ShowIf("showEvents")] public UnityEvent onWindowOpen;
        [ShowIf("showEvents")] public UnityEvent onWindowClose;

        [Button]
        async public void OpenWindow() => await OpenWindowAsync();

        async public UniTask OpenWindowAsync() {
            if (CurrentState.status == WindowStatus.Opened || CurrentState.status == WindowStatus.Opening) return;

            CurrentState.status = WindowStatus.Opening;
            var transitions = transitioners.Where(x => x.enabled)
                .Select(x => x.TransitionInAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            CurrentState.status = WindowStatus.Opened;

            WindowOpened();
            onWindowOpen?.Invoke();
        }

        [Button]
        async public void CloseWindow() => await CloseWindowAsync();

        async public UniTask CloseWindowAsync() {
            if (CurrentState.status == WindowStatus.Closed || CurrentState.status == WindowStatus.Closing) return;

            CurrentState.status = WindowStatus.Closing;
            var transitions = transitioners.Where(x => x.enabled)
                .Select(x => x.TransitionOutAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            CurrentState.status = WindowStatus.Closed;
            
            WindowClosed();
            onWindowClose?.Invoke();
        }

        sealed protected override void HandleViewStateSet() {
            // TODO: Make Status is a reactive property and react to changes
            HandleWindowStateSet();
        }

        protected abstract void HandleWindowStateSet();
        protected virtual void WindowOpened() { }
        protected virtual void WindowClosed() { }
    }
}