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
        [SerializeField] WindowStatus status;
        [SerializeField] TweenerBase[] tweeners;

        /// <summary>
        /// The current status of the window
        /// </summary>
        public WindowStatus Status {
            get => status;
            private set => status = value;
        }

        /// <summary>
        /// Returns true if the window is open or currently opening
        /// </summary>
        public bool IsOpenOrOpening =>
            Status == WindowStatus.Opened || Status == WindowStatus.Opening;

        /// <summary>
        /// Returns true if the window is closed or currently closing
        /// </summary>
        public bool IsClosedOrOpening =>
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
            var navigation = GetComponentInParent<Navigator>();
            if (navigation != null)
                navigation.RegisterWindow(this);
        }

        /// <summary>
        /// Opens the window and returns a task that completes when it's done opening
        /// </summary>
        async public UniTask OpenWindowAsync() {
            await UniTask.SwitchToMainThread();
            if (Status == WindowStatus.Opened || Status == WindowStatus.Opening) return;

            OnWindowStartOpening();
            onWindowStartOpening?.Invoke();

            Status = WindowStatus.Opening;
            if (tweeners.Length == 0)
                tweeners = Tweeners;
            var transitions = tweeners.Where(x => x.enabled)
                .Select(x => x.TweenInAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            Status = WindowStatus.Opened;

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
            if (Status == WindowStatus.Closed || Status == WindowStatus.Closing) return;

            OnWindowStartClosing();
            onWindowStartClosing?.Invoke();

            Status = WindowStatus.Closing;
            if (tweeners.Length == 0)
                tweeners = Tweeners;
            var transitions = tweeners.Where(x => x.enabled)
                .Select(x => x.TweenOutAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            Status = WindowStatus.Closed;

            OnWindowDoneClosing();
            onWindowDoneClosing?.Invoke();
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