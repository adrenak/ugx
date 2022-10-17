using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace Adrenak.UGX {
    /// <summary>
    /// Keeps track of whether a UI is opening, opened, closing, closed.
    /// Uses Tweeners to open (Transition Up) and close (Transition Down)
    /// </summary>
    public class Window : UGXBehaviour {
        // TODO: Get rid of these two
        public Sprite icon;
        public string title;

        [SerializeField] bool dontTweenToSameStatus = true;
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

        List<CancellationTokenSource> cancellationSources;

        void Awake() {
            cancellationSources = new List<CancellationTokenSource>();
            // Add this window to a parent navigator, if present
            var navigator = transform.parent.GetComponent<Navigator>();
            if (navigator != null)
                navigator.SendMessage("RegisterWindow", this);
        }

        /// <summary>
        /// Opens the window. Completion is awaitable.
        /// </summary>
        async public UniTask OpenWindowAsync() {
            if (status == WindowStatus.Opened && !dontTweenToSameStatus) 
                return;

            status = WindowStatus.Opening;
            OnWindowStartOpening();
            WindowStartedOpening?.Invoke();

            if (activeTweeners.Length == 0)
                activeTweeners = Tweeners;

            if (cancellationSources.Count > 0) {
                cancellationSources.ForEach(x => x.Cancel());
                cancellationSources.Clear();
            }

            var transitions = activeTweeners
                .Select(x => {
                    var cancelSource = new CancellationTokenSource();
                    cancellationSources.Add(cancelSource);
                    return x.TweenInAsync(cancelSource.Token);
                })
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            cancellationSources.Clear();
            status = WindowStatus.Opened;

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
            if (status == WindowStatus.Closed && !dontTweenToSameStatus) 
                return;

            status = WindowStatus.Closing;
            OnWindowStartClosing();
            WindowStartedClosing?.Invoke();

            if (cancellationSources.Count > 0) {
                cancellationSources.ForEach(x => x.Cancel());
                cancellationSources.Clear();
            }

            if (activeTweeners.Length == 0)
                activeTweeners = Tweeners;
                var transitions = activeTweeners
                .Select(x => {
                    var cancelSource = new CancellationTokenSource();
                    cancellationSources.Add(cancelSource);
                    return x.TweenOutAsync(cancelSource.Token);
                })
                .ToList();

            await UniTask.WhenAll(transitions);
            await UniTask.SwitchToMainThread();
            cancellationSources.Clear();
            status = WindowStatus.Closed;

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