using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log($"Could not find any element with name {name}");
        return null;
    }

    public void DisplayAlert(object content) {
        Debug.Log(content);
    }
}
