using Adrenak.UPF.Examples.Unigram;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.Events;

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
    }

    [Serializable]
    public class Page : BindableBehaviour {
#pragma warning disable 0649
        [SerializeField] UnityEvent onPageOpen;
        [SerializeField] UnityEvent onPageClose;

        [SerializeField] protected Navigator navigator;
        [ReadOnly] [SerializeField] bool isOpen;
        public bool IsOpen => isOpen;
#pragma warning restore 0649

        void Start() {
            OnStartPage();
        }

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

        protected virtual void OnStartPage() { }
        protected virtual void OnSetPageModel() { }
        protected virtual void OnPageModelPropertyChanged(string propertyName) { }

        protected virtual void OnBackButtonPress() { }
        protected virtual void OnPageOpen() { }
        protected virtual void OnPageClose() { }
    }
}
