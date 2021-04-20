using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;

namespace Adrenak.UGX {
    public class Window : UGXBehaviour {
        [ReadOnly] public WindowStatus status;
        public bool autoPopOnBack = true;

        [BoxGroup("Window Events")] [SerializeField] bool showEvents;
        [BoxGroup("Window Events")] [ShowIf("showEvents")] public UnityEvent onWindowStartOpening;
        [BoxGroup("Window Events")] [ShowIf("showEvents")] public UnityEvent onWindowDoneOpening;
        [BoxGroup("Window Events")] [ShowIf("showEvents")] public UnityEvent onWindowStartClosing;
        [BoxGroup("Window Events")] [ShowIf("showEvents")] public UnityEvent onWindowDoneClosing;

        [Button]
        async public void OpenWindow() => await OpenWindowAsync();

        async public UniTask OpenWindowAsync() {
            await UniTask.SwitchToMainThread();
            if (status == WindowStatus.Opened || status == WindowStatus.Opening) return;

            onWindowStartOpening?.Invoke();

            status = WindowStatus.Opening;
            var transitions = transitioners.Where(x => x.enabled)
                .Select(x => x.TransitionInAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            status = WindowStatus.Opened;

            WindowOpened();
            onWindowDoneOpening?.Invoke();
        }

        [Button]
        async public void CloseWindow() => await CloseWindowAsync();

        async public UniTask CloseWindowAsync() {
            await UniTask.SwitchToMainThread();
            if (status == WindowStatus.Closed || status == WindowStatus.Closing) return;

            onWindowStartClosing?.Invoke();

            status = WindowStatus.Closing;
            var transitions = transitioners.Where(x => x.enabled)
                .Select(x => x.TransitionOutAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            status = WindowStatus.Closed;

            WindowClosed();
            onWindowDoneClosing?.Invoke();
        }

        public virtual UniTask<bool> ApprovePop() {
            return UniTask.FromResult(true);
        }

        public virtual UniTask<bool> ApprovePush() {
            return UniTask.FromResult(true);
        }

        protected virtual void WindowOpened() { }
        protected virtual void WindowClosed() { }
    }
}