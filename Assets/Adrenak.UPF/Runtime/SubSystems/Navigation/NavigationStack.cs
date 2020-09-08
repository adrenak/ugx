using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Adrenak.UPF {
    /// <summary>
    /// This class is not marker abstract for serialization purposes, but it needs to be subclassed.
    /// See DefaultNavigationStack for an example.
    /// </summary>
    [Serializable]
    public class NavigationStack {
        [ReadOnly] [SerializeField] protected Window current = null;
        public Window Current => current;

        [ReadOnly] [ReorderableList] [SerializeField] List<Window> _history = new List<Window>();
        public List<Window> History => _history;

        public virtual void Push(Window page) => throw new NotImplementedException();
        public virtual void Pop() => throw new NotImplementedException();
    }
}

