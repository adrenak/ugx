using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;

namespace Adrenak.UGX {
    /// <summary>
    /// Keeps track of whether a UI is opening, opened, closing, closed.
    /// Uses Tweeners to open (Transition Up) and close (Transition Down)
    /// </summary>
    public class Window : UGXBehaviour {
        [ReadOnly] [SerializeField] WindowStatus status;
        /// <summary>
        /// The current status of the window
        /// </summary>
        public WindowStatus Status {
            get => status;
            private set => status = value; 
        }

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
        [Button]
        async public void OpenWindow() => await OpenWindowAsync();

        /// <summary>
        /// Opens the window and returns a task that completes when it's done opening
        /// </summary>
        async public UniTask OpenWindowAsync() {
            await UniTask.SwitchToMainThread();
            if (Status == WindowStatus.Opened || Status == WindowStatus.Opening) return;

            OnWindowStartOpening();
            onWindowStartOpening?.Invoke();

            Status = WindowStatus.Opening;
            var transitions = tweeners.Where(x => x.enabled)
                .Select(x => x.TransitionInAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            Status = WindowStatus.Opened;

            OnWindowDoneOpening();
            onWindowDoneOpening?.Invoke();
#pragma warning disable 0618
            WindowOpened();
#pragma warning restore 0618
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        [Button]
        async public void CloseWindow() => await CloseWindowAsync();

        /// <summary>
        /// Closes the window and returns a task that completes when it's done closing
        /// </summary>
        async public UniTask CloseWindowAsync() {
            await UniTask.SwitchToMainThread();
            if (Status == WindowStatus.Closed || Status == WindowStatus.Closing) return;

            OnWindowStartClosing();
            onWindowStartClosing?.Invoke();

            Status = WindowStatus.Closing;
            var transitions = tweeners.Where(x => x.enabled)
                .Select(x => x.TransitionOutAsync())
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            Status = WindowStatus.Closed;

            OnWindowDoneClosing();
            onWindowDoneClosing?.Invoke();
#pragma warning disable 0618
            WindowClosed();
#pragma warning restore 0618
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

#region OBSOLETE
        /// <summary>
        /// Called when the Window finishes opening
        /// </summary>
        [Obsolete("Use OnWindowDoneOpening instead")]
        protected virtual void WindowOpened() { }

        /// <summary>
        /// Called when the Window finishes closing
        /// </summary>
        [Obsolete("Use OnWindowDoneClosing instead")]
        protected virtual void WindowClosed() { }
#endregion
    }
}