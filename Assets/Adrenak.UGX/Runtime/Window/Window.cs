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
        public UnityEvent WindowStartedOpening;

        /// <summary>
        /// Fired when the window is finished opening
        /// </summary>
        public UnityEvent WindowDoneOpening;

        /// <summary>
        /// Fired when the window starts closing
        /// </summary>
        public UnityEvent WindowStartedClosing;

        /// <summary>
        /// Fired when the window is finished closing
        /// </summary>
        public UnityEvent WindowDoneClosing;

        /// <summary>
        /// Opens the window
        /// </summary>
        public void OpenWindow() => OpenWindowAsync();

        void Awake() {
            // Add this window to a parent navigator, if present
            var navigator = transform.parent.GetComponent<Navigator>();
            if (navigator != null)
                navigator.SendMessage("RegisterWindow", this);
        }

        /// <summary>
        /// Opens the window. Completion is awaitable.
        /// </summary>
        async public UniTask OpenWindowAsync() {
            if (IsOpenOrOpening) return;

            while (status == WindowStatus.Closing)
                await UniTask.Yield();
            await UniTask.SwitchToMainThread();

            status = WindowStatus.Opening;
            OnWindowStartOpening();
            WindowStartedOpening?.Invoke();

            if (activeTweeners.Length == 0)
                activeTweeners = Tweeners;

            var transitions = activeTweeners.Where(x => x.enabled)
                .Select(x => x.TweenInAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            status = WindowStatus.Opened;

            await UniTask.SwitchToMainThread();
            OnWindowDoneOpening();
            WindowDoneOpening?.Invoke();
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        public void CloseWindow() => CloseWindowAsync();

        /// <summary>
        /// Closes the window. Completion is awaitable.
        /// </summary>
        async public UniTask CloseWindowAsync() {
            if (IsClosedOrClosing) return;

            while (status == WindowStatus.Opening)
                await UniTask.Yield();
            await UniTask.SwitchToMainThread();

            status = WindowStatus.Closing;
            OnWindowStartClosing();
            WindowStartedClosing?.Invoke();

            if (activeTweeners.Length == 0)
                activeTweeners = Tweeners;
            var transitions = activeTweeners.Where(x => x.enabled)
                .Select(x => x.TweenOutAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            status = WindowStatus.Closed;

            await UniTask.SwitchToMainThread();
            OnWindowDoneClosing();
            WindowDoneClosing?.Invoke();
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