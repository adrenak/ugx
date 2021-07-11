using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    /// <summary>
    /// Keeps track of whether a UI is opening, opened, closing, closed.
    /// Uses Tweeners to open (Transition Up) and close (Transition Down)
    /// </summary>
    public class Window : UGXBehaviour {
        public Sprite icon;
        public string title;

        public bool canReopenWhileOpen;
        public bool canReopenWhileOpening;
        public bool canRecloseWhileClosed;
        public bool canRecloseWhileClosing;

        [SerializeField] WindowStatus status;
        [SerializeField] TweenerBase[] activeTweeners;

        /// <summary>
        /// The current status of the window
        /// </summary>
        public WindowStatus Status => status;

        /// <summary>
        /// Array of <see cref="TweenerBase"/> used by the window for animation
        /// </summary>
        public TweenerBase[] ActiveTweeners => activeTweeners;

        /// <summary>
        /// Returns true if the window is open or currently opening
        /// </summary>
        public bool IsOpenOrOpening =>
            Status == WindowStatus.Opened || Status == WindowStatus.Opening;

        /// <summary>
        /// Returns true if the window is closed or currently closing
        /// </summary>
        public bool IsClosedOrClosing =>
            Status == WindowStatus.Closed || Status == WindowStatus.Closing;

        /// <summary>
        /// Fired when the window starts opening
        /// </summary>
        public UnityEvent onWindowStartOpening;

        /// <summary>
        /// Fired when the window is finished opening
        /// </summary>
        public UnityEvent onWindowDoneOpening;

        /// <summary>
        /// Fired when the window starts closing
        /// </summary>
        public UnityEvent onWindowStartClosing;

        /// <summary>
        /// Fired when the window is finished closing
        /// </summary>
        public UnityEvent onWindowDoneClosing;

        /// <summary>
        /// Opens the window
        /// </summary>
        async public void OpenWindow() => await OpenWindowAsync();

        void Awake() {
            var parent = transform.parent;
            var navigator = parent.GetComponent<Navigator>();
            if (navigator != null)
                navigator.RegisterWindow(this);
        }

        /// <summary>
        /// Opens the window and returns a task that completes when it's done opening
        /// </summary>
        async public UniTask OpenWindowAsync() {
            await UniTask.SwitchToMainThread();
            if (Status == WindowStatus.Opened && !canReopenWhileOpen) return;
            if (Status == WindowStatus.Opening && !canReopenWhileOpening) return;
            if (!(await AllowOpeningWindow())) return;

            status = WindowStatus.Opening;
            OnWindowStartOpening();
            onWindowStartOpening?.Invoke();

            if (activeTweeners.Length == 0)
                activeTweeners = Tweeners;
            var transitions = activeTweeners.Where(x => x.enabled)
                .Select(x => x.TweenInAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            status = WindowStatus.Opened;

            OnWindowDoneOpening();
            onWindowDoneOpening?.Invoke();
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        async public void CloseWindow() => await CloseWindowAsync();

        /// <summary>
        /// Closes the window and returns a task that completes when it's done closing
        /// </summary>
        async public UniTask CloseWindowAsync() {
            if (Status == WindowStatus.Closed && !canRecloseWhileClosed) return;
            if (Status == WindowStatus.Closing && !canRecloseWhileClosing) return;
            if (!(await AllowClosingWindow())) return;

            status = WindowStatus.Closing;
            OnWindowStartClosing();
            onWindowStartClosing?.Invoke();

            if (activeTweeners.Length == 0)
                activeTweeners = Tweeners;
            var transitions = activeTweeners.Where(x => x.enabled)
                .Select(x => x.TweenOutAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            status = WindowStatus.Closed;

            OnWindowDoneClosing();
            onWindowDoneClosing?.Invoke();
        }

        protected virtual UniTask<bool> AllowClosingWindow() {
            return UniTask.FromResult(true);
        }

        protected virtual UniTask<bool> AllowOpeningWindow() {
            return UniTask.FromResult(true);
        }

        /// <summary>
        /// Called when the Window starts opening
        /// </summary>
        protected virtual void OnWindowStartOpening() { }

        /// <summary>
        /// Called when the Window finishes opening
        /// </summary>
        protected virtual void OnWindowDoneOpening() { }

        /// <summary>
        /// Called when the Window starts closing
        /// </summary>
        protected virtual void OnWindowStartClosing() { }

        /// <summary>
        /// Called when the Window finishes closing
        /// </summary>
        protected virtual void OnWindowDoneClosing() { }
    }
}