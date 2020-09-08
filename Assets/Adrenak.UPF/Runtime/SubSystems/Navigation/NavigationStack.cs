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
        [ReadOnly] [SerializeField] protected Page current = null;
        public Page Current => current;

        [ReadOnly] [ReorderableList] [SerializeField] List<Page> _history = new List<Page>();
        public List<Page> History => _history;

        public virtual void Push(Page page) => throw new NotImplementedException();
        public virtual void Pop() => throw new NotImplementedException();
    }
}

