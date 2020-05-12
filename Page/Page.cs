using UnityEngine;


namespace Adrenak.UPF {
    
    public abstract class Page : BindableBehaviour {
        [SerializeField] string title;
        
        public string Title {
            get => title;
            set => Set(ref title, value);
        }

        [SerializeField] Navigator navigation;
        
        public Navigator Navigation => navigation;

        void Start() {
            InitializePage();
        }

        public T GetPageElement<T>(string name) where T : Component {
            for (int i = 0; i < transform.childCount; i++) {
                var child = transform.GetChild(i).gameObject;
                if (child.name.Equals(name))
                    return child.GetComponent<T>();
            }
            Debug.LogError($"Could not find any element with name {name} in page {gameObject.name}");
            return null;
        }

        public void DisplayAlert(object content) {
            Debug.Log(content);
        }

        protected virtual void InitializePage() { }
        public virtual void OnAppearing() { }
        public virtual void OnDisappearing() { }
        public virtual void OnBackButtonPress() { }
    }
}
