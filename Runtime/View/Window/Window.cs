using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;

namespace Adrenak.UGX {
    public class Window : Window<WindowState> {
        protected override void HandleWindowStateSet() { }
    }

    [System.Serializable]
    public class WindowState : ViewState {
        public WindowStatus status;
        public bool autoPopOnBack = true;
    }

    public abstract class Window<T> : View<T> where T : WindowState {
        [BoxGroup("Window Events")] [SerializeField] bool showEvents;
        [BoxGroup("Window Events")] [ShowIf("showEvents")] public UnityEvent onWindowStartOpening;
        [BoxGroup("Window Events")] [ShowIf("showEvents")] public UnityEvent onWindowDoneOpening;
        [BoxGroup("Window Events")] [ShowIf("showEvents")] public UnityEvent onWindowStartClosing;
        [BoxGroup("Window Events")] [ShowIf("showEvents")] public UnityEvent onWindowDoneClosing;

        [Button]
        async public void OpenWindow() => await OpenWindowAsync();

        async public UniTask OpenWindowAsync() {
            await UniTask.SwitchToMainThread();
            if (CurrentState.status == WindowStatus.Opened || CurrentState.status == WindowStatus.Opening) return;

            onWindowStartOpening?.Invoke();

            CurrentState.status = WindowStatus.Opening;
            var transitions = transitioners.Where(x => x.enabled)
                .Select(x => x.TransitionInAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            CurrentState.status = WindowStatus.Opened;

            WindowOpened();
            onWindowDoneOpening?.Invoke();
        }

        [Button]
        async public void CloseWindow() => await CloseWindowAsync();

        async public UniTask CloseWindowAsync() {
            await UniTask.SwitchToMainThread();
            if (CurrentState.status == WindowStatus.Closed || CurrentState.status == WindowStatus.Closing) return;

            onWindowStartClosing?.Invoke();

            CurrentState.status = WindowStatus.Closing;
            var transitions = transitioners.Where(x => x.enabled)
                .Select(x => x.TransitionOutAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            CurrentState.status = WindowStatus.Closed;

            WindowClosed();
            onWindowDoneClosing?.Invoke();
        }

        public virtual UniTask<bool> ApprovePop() {
            return UniTask.FromResult(true);
        }

        public virtual UniTask<bool> ApprovePush() {
            return UniTask.FromResult(true);
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