using System;
using UnityEngine;

public class BindingContext : MonoBehaviour {
    public event Action OnDataChange;
    [SerializeField] string typeName;

    public string TypeName {
        get => typeName;
    }

    public Type Type {
        get {
            try {
                return Type.GetType(typeName);
            }
            catch (Exception e) {
                throw e;
            }
        }
    }

    object data;
    public object Data {
        get => data;
        set {
            data = value;
            HandleDataChange();
            OnDataChange?.Invoke();
        }
    }

    public void HandleDataChange() {
        data = Convert.ChangeType(data, Type);
    }
}
