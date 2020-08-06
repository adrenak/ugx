using System;

using UnityEngine;
using UnityEngine.Events;

using NaughtyAttributes;
using Adrenak.Unex;

namespace Adrenak.UPF {
    [Serializable]
    public abstract class Page<T> : Page where T : PageModel {
        [SerializeField] T model;
        public T Model {
            get => model;
            set {
                model = value ?? throw new ArgumentNullException(nameof(Model));
                OnSetPageModel();
            }
        }

        protected virtual void OnSetPageModel() { }
        protected virtual void OnPageModelPropertyChanged(string propertyName) { }
    }

    [Serializable]
    public class Page : View {
#pragma warning disable 0649
        [Header("UNITY EVENTS")]
        [SerializeField] bool showEvents;
        [ShowIf("showEvents")] public UnityEvent onPageOpen;
        [ShowIf("showEvents")] public UnityEvent onPageClose;
        [ShowIf("showEvents")] public UnityEvent onPressBack;

        [HorizontalLine(color:EColor.Blue)]
        [SerializeField] protected Navigator navigator;
        [ReadOnly] [SerializeField] bool isOpen;
#pragma warning restore 0649

        public bool IsOpen => isOpen;
        bool isOpening, isClosing;

        public void OpenPage(bool silently = false) {
            if (isOpen || isOpening) return;

            isOpening = true;
            Dispatcher.Enqueue(() => {
                isOpen = true;
                isOpening = false;
            });

            OnPageOpen();
            if (!silently)
                onPageOpen?.Invoke();
        }

        public void ClosePage(bool silently = false) {
            if (!isOpen || isClosing) return;

            isClosing = true;
            Dispatcher.Enqueue(() => {
                isOpen = false;
                isClosing = false;
            });

            OnPageClose();
            if (!silently)
                onPageClose?.Invoke();
        }

        /// <summary>
        /// If you're overriding this, make sure to call base.Update() in your subclass
        /// </summary>
        protected void Update() {
            CheckBackPress();
        }

        void CheckBackPress(){
            if (Input.GetKeyUp(KeyCode.Escape) && IsOpen) {
                navigator.Pop();
                onPressBack?.Invoke();
                OnPressBack();
            }
        }

        protected virtual void OnPageOpen() { }
        protected virtual void OnPageClose() { }
        protected virtual void OnPressBack() { }
    }
}
