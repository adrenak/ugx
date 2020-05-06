using System;
using UnityEngine;

public abstract class ListItem : BindingSource {
    public event Action OnClick;

    public void Click() {
        OnClick?.Invoke();
    }

    public virtual void Set(BindingModel data) {
        Model = data;
    }
}
