using System;
using UnityEngine;
using UnityEngine.Events;

namespace Adrenak.UPF {
    [Serializable]
    public class Page : View {
#pragma warning disable 0649
        [SerializeField] UnityEvent onAppear;
        [SerializeField] UnityEvent onDisappear;

        [SerializeField] string title;
        public string Title {
            get => title;
            set => Set(ref title, value);
        }

        [SerializeField] Navigator navigator;
        public Navigator Navigator => navigator;
#pragma warning restore 0649

        void Start() {
            OnInitializePage();
        }

        protected virtual void OnAppear() { }
        public void Appear() {
            OnAppear();
            onAppear?.Invoke();
        }

        protected virtual void OnDisappear() { }
        public void Disappear() {
            OnDisappear();
            onDisappear?.Invoke();
        }

        public virtual void OnBackButtonPress() { }
        protected virtual void OnInitializePage() { }
    }
}
