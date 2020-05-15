using Adrenak.UPF.Examples.Unigram;
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
        [SerializeField] UnityEvent onAppear;
        [SerializeField] UnityEvent onDisappear;

        [SerializeField] protected View content;
        public View Content => content;

        void Start() {
            OnStart();
        }

        public void AppearPage() {
            OnAppear();
            onAppear?.Invoke();
        }

        public void DisappearPage() {
            OnDisappear();
            onDisappear?.Invoke();
        }

        protected virtual void OnStart() { }
        protected virtual void OnInitializePage() { }
        protected virtual void OnPageModelPropertyChanged(string propertyName) { }
        
        protected virtual void OnBackButtonPress() { }
        protected virtual void OnAppear() { }
        protected virtual void OnDisappear() { }
    }
}
