using UnityEngine;

namespace Adrenak.UPF {
    public abstract class Page : MonoBehaviour {
        protected abstract void InitializePage();

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
    }
}
