using Adrenak.UPF.Examples.Unigram;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Adrenak.UPF {
    [Serializable]
    public abstract class PageView<T> : PageView where T : PageModel {
        [SerializeField] T model;
        public T Model {
            get => model;
            set {
                model = value ?? throw new ArgumentNullException("Model");
                OnInitializePage();
            }
        }
    }

    [Serializable]
    public class PageView : BindableBehaviour {
#pragma warning disable 0649
        [SerializeField] UnityEvent onAppear;
        [SerializeField] UnityEvent onDisappear;

        [ReadOnly] [SerializeField] bool isOpen;
        public bool IsOpen => isOpen;

        [SerializeField] protected View content;
        public View Content => content;
#pragma warning restore 0649

        void Start() {
            OnStart();
        }

        public void OpenPage() {
            isOpen = true;
            OnPageOpen();
            onAppear?.Invoke();
        }

        public void ClosePage() {
            isOpen = false;
            OnPageClose();
            onDisappear?.Invoke();
        }

        protected virtual void OnStart() { }
        protected virtual void OnInitializePage() { }
        protected virtual void OnPageModelPropertyChanged(string propertyName) { }
        
        protected virtual void OnBackButtonPress() { }
        protected virtual void OnPageOpen() { }
        protected virtual void OnPageClose() { }
    }
}
