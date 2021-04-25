using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;

namespace Adrenak.UGX {
    public class Window : UGXBehaviour {
        [ReadOnly] [SerializeField] WindowStatus status;
        public WindowStatus Status {
            get => status;
            private set => status = value; 
        }

        public bool autoPopOnBack = true;

        public UnityEvent onWindowStartOpening;
        public UnityEvent onWindowDoneOpening;
        public UnityEvent onWindowStartClosing;
        public UnityEvent onWindowDoneClosing;

        [Button]
        async public void OpenWindow() => await OpenWindowAsync();

        async public UniTask OpenWindowAsync() {
            await UniTask.SwitchToMainThread();
            if (Status == WindowStatus.Opened || Status == WindowStatus.Opening) return;

            onWindowStartOpening?.Invoke();

            Status = WindowStatus.Opening;
            var transitions = transitioners.Where(x => x.enabled)
                .Select(x => x.TransitionInAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            Status = WindowStatus.Opened;

            WindowOpened();
            onWindowDoneOpening?.Invoke();
        }

        [Button]
        async public void CloseWindow() => await CloseWindowAsync();

        async public UniTask CloseWindowAsync() {
            await UniTask.SwitchToMainThread();
            if (Status == WindowStatus.Closed || Status == WindowStatus.Closing) return;

            onWindowStartClosing?.Invoke();

            Status = WindowStatus.Closing;
            var transitions = transitioners.Where(x => x.enabled)
                .Select(x => x.TransitionOutAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            Status = WindowStatus.Closed;

            WindowClosed();
            onWindowDoneClosing?.Invoke();
        }

        protected virtual void WindowOpened() { }
        protected virtual void WindowClosed() { }
    }
}