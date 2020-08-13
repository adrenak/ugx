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
        [ReadOnly] [SerializeField] protected View current = null;
        public View Current => current;

        [ReadOnly] [ReorderableList] [SerializeField] List<View> _history = new List<View>();
        public List<View> History => _history;

        public virtual void Push(View page) => throw new NotImplementedException();
        public virtual void Pop() => throw new NotImplementedException();
    }
}

