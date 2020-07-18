using System;

using UnityEngine;
using UnityEngine.Events;

using NaughtyAttributes;

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
    public class Page : BindableBehaviour {
#pragma warning disable 0649
        [SerializeField] UnityEvent onPageOpen;
        [SerializeField] UnityEvent onPageClose;
        [SerializeField] UnityEvent onPressBack;

        [SerializeField] protected Navigator navigator;
        [ReadOnly] [SerializeField] bool isOpen;
        public bool IsOpen => isOpen;
#pragma warning restore 0649

        public void OpenPage() {
            isOpen = true;
            OnPageOpen();
            onPageOpen?.Invoke();
        }

        public void ClosePage() {
            isOpen = false;
            OnPageClose();
            onPageClose?.Invoke();
        }

        void Update() {
            if (Input.GetKeyUp(KeyCode.Escape) && IsOpen) {
                OnPressBack();
                onPressBack.Invoke();
            }
        }

        protected virtual void OnPageOpen() { }
        protected virtual void OnPageClose() { }
        protected virtual void OnPressBack() { }
    }
}
