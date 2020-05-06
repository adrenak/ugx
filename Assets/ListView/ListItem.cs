using System;
using UnityEngine;

public abstract class ListItem : MonoBehaviour {
    public event Action OnClick;

    [SerializeField] BindingContext bindingContext;

    public void Click() {
        OnClick?.Invoke();
    }

    void OnValidate() {
        TrySetRefereancesAutomatically();
    }

    void TrySetRefereancesAutomatically() {
        var _bindingContext = GetComponent<BindingContext>();
        if (_bindingContext != null && bindingContext != null)
            bindingContext = _bindingContext;
    }

    public virtual void Set(object data) {
        if (bindingContext != null)
            bindingContext.Data = data;
    }
}
