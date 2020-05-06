using System;
using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using JetBrains.Annotations;
using System.Reflection;

[Serializable]
public class BindingPair {
    [SerializeField] string comment;
    public BindingMember destination;
    public BindingMember source;
}

[Serializable]
public enum MemberType {
    Field,
    Property
}

[Serializable]
public class BindingMember {
    public string path;
    public MemberType type;
}

[Serializable]
public abstract class Binding<T> : MonoBehaviour where T : Component {
    [SerializeField] [ReorderableList] List<BindingPair> pairs;
    [SerializeField] BindingContext context;
    [SerializeField] protected T destination;

    void Start() {
        if (context != null) {
            if (context.Data != null)
                Pull();
            context.OnDataChange += Pull;
        }
    }

    public void Pull() {
        try {
            foreach (var update in pairs) {
                try {
                    MemberInfo dest = destination.GetType().GetMember(update.destination.path)[0];
                    MemberInfo source = context.Data.GetType().GetMember(update.source.path)[0];

                    if (update.destination.type == MemberType.Property) {
                        if (update.source.type == MemberType.Property)
                            (dest as PropertyInfo).SetValue(
                                destination,
                                (source as PropertyInfo).GetValue(context.Data)
                            );
                        else
                            (dest as PropertyInfo).SetValue(
                                destination,
                                (source as FieldInfo).GetValue(context.Data)
                            );
                    }
                    else {
                        if (update.source.type == MemberType.Property)
                            (dest as FieldInfo).SetValue(
                                destination,
                                (source as PropertyInfo).GetValue(context.Data)
                            );
                        else
                            (dest as FieldInfo).SetValue(
                                destination,
                                (source as FieldInfo).GetValue(context.Data)
                            );
                    }
                }
                catch (Exception e) {
                    Debug.LogError(e);
                }
            }
        }
        catch (Exception e) {
            throw e;
        }
    }

    private void OnValidate() {
        TrySetReferencesAutomatically();
    }

    void TrySetReferencesAutomatically() {
        var _context = GetComponentInParent<BindingContext>();
        if (_context != null && context == null)
            context = _context;

        var _destination = GetComponent<T>();
        if (_destination != null && destination == null)
            destination = _destination;
    }
}
