using System;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
public abstract class BindingReference<T> : MonoBehaviour where T : Component {
    [SerializeField] BindingContext context;
    [SerializeField] protected T destination;
    [SerializeField] string sourceField;

    void Start() {
        if (context != null) {
            if (context.Data != null)
                Pull();
            context.OnSetData += Pull;
        }
    }

    private void OnValidate() {
        TrySetReferencesAutomatically();
    }

    void TrySetReferencesAutomatically(){
        var _context = GetComponentInParent<BindingContext>();
        if (_context != null && context == null)
            context = _context;

        var _destination = GetComponent<T>();
        if (_destination != null && destination == null)
            destination = _destination;
    }

    public void Pull() {
        try {
            var field = context.Type.GetField(sourceField).GetValue(context.Data);
            OnPulled(field);
        }
        catch (Exception e) {
            throw e;
        }
    }

    public abstract void OnPulled(object obj);
}
