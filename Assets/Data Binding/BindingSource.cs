using System;
using UnityEngine;

public class BindingSource : MonoBehaviour {
    public event Action OnDataChange;
    public event Action<string> OnMemberChange;
    [SerializeField] string bindingTypeName;

    public string BingingTypeName {
        get => bindingTypeName;
    }

    public Type Type {
        get {
            try {
                var type = Type.GetType(bindingTypeName);
                if (type == null)
                    throw new Exception($"Could not find type {bindingTypeName}");
                return type;
            }
            catch (Exception e) {
                throw e;
            }
        }
    }

    BindingModel model;
    public BindingModel Model {
        get => model;
        set {
            model = value;
            model.Updated += OnMemberChange;
            HandleDataChange();
            OnDataChange?.Invoke();
        }
    }

    public void HandleDataChange() {
        model = Convert.ChangeType(model, Type) as BindingModel;
    }
}
