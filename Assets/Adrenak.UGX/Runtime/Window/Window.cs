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

        [SerializeField] bool dontChangeToSameEndStatus = true;
        [SerializeField] bool dontChangeAlongSameTransitoryStatus = true;
        [SerializeField] WindowStatus status;

        /// <summary>
        /// Whether the window should 
        /// </summary>
        public bool DontChangeToSameEndStatus {
            get => dontChangeToSameEndStatus;
            set => dontChangeToSameEndStatus = value;
        }

        /// <summary>
        /// Whether the window should tween along the same sta
        /// </summary>
        public bool DontChangeAlongSameTransitoryStatus {
            get => dontChangeAlongSameTransitoryStatus;
            set => dontChangeAlongSameTransitoryStatus = value;
        }

        /// <summary>
        /// The current status of the window
        /// </summary>
        public WindowStatus Status => status;

        /// <summary>
        /// If the window is currently open
        /// </summary>
        public bool IsOpened => Status == WindowStatus.Opened;

        /// <summary>
        /// If the window is currently opening
        /// </summary>
        public bool IsOpening => Status == WindowStatus.Opening;

        /// <summary>
        /// If the window is currently closed
        /// </summary>
        public bool IsClosed => Status == WindowStatus.Closed;

        /// <summary>
        /// If the window is currently closing
        /// </summary>
        public bool IsClosing => Status == WindowStatus.Closing;

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

        List<CancellationTokenSource> cancellationSources = new List<CancellationTokenSource>();

        /// <summary>
        /// Opens the window
        /// </summary>
        public void OpenWindow(bool force = false) => OpenWindowAsync(force);

        /// <summary>
        /// Opens the window. Completion is awaitable.
        /// </summary>
        async public UniTask OpenWindowAsync(bool force = false) {
            if (status == WindowStatus.Opened && !dontChangeToSameEndStatus && !force)
                return;
            if (status == WindowStatus.Opening && !dontChangeAlongSameTransitoryStatus && !force)
                return;

            status = WindowStatus.Opening;
            OnWindowStartOpening();
            WindowStartedOpening?.Invoke();

            if (cancellationSources.Count > 0) {
                cancellationSources.ForEach(x => x.Cancel());
                cancellationSources.Clear();
            }

            var transitions = Tweeners
                .Where(x => x.enabled)
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
        public void CloseWindow(bool force = false) => CloseWindowAsync(force);

        /// <summary>
        /// Closes the window. Completion is awaitable.
        /// </summary>
        async public UniTask CloseWindowAsync(bool force = false) {
            if (status == WindowStatus.Closed && dontChangeToSameEndStatus && !force)
                return;
            if (status == WindowStatus.Closing && dontChangeAlongSameTransitoryStatus && !force)
                return;

            status = WindowStatus.Closing;
            OnWindowStartClosing();
            WindowStartedClosing?.Invoke();

            if (cancellationSources.Count > 0) {
                cancellationSources.ForEach(x => x.Cancel());
                cancellationSources.Clear();
            }

            var transitions = Tweeners
                .Where(x => x.enabled)
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