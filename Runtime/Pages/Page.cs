using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using NaughtyAttributes;
using Adrenak.Unex;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

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
        public UnityEvent onPageOpen;
        public UnityEvent onPageClose;
        public UnityEvent onPressBack;

        [SerializeField] protected Navigator navigator;
        [ReadOnly] [SerializeField] bool isOpen;
        public bool IsOpen => isOpen;
#pragma warning restore 0649

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

        void Update() {
            OnPageUpdate();
            if (Input.GetKeyUp(KeyCode.Escape) && IsOpen) {
                onPressBack?.Invoke();
                OnPressBack();
            }
        }

        protected virtual void OnPageOpen() { }
        protected virtual void OnPageUpdate() { }
        protected virtual void OnPageClose() { }
        protected virtual void OnPressBack() { }
    }
}
