using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// This class is not marked abstract for serialization purposes, but it needs to be subclassed.
    /// See DefaultNavigationStack for an example.
    /// </summary>
    [Serializable]
    public class NavigationStack {
        [ReadOnly] [SerializeField] protected Window current = null;
        public Window Current => current;

        [ReadOnly] [ReorderableList] [SerializeField] List<Window> _history = new List<Window>();
        public List<Window> History => _history;

        public virtual void Push(Window window) => throw new NotImplementedException("You need to set Navigator.Stack to an implementation of NavigationStack class");
        public virtual void Pop() => throw new NotImplementedException("You need to set Navigator.Stack to an implementation of NavigationStack class");
    }
}

