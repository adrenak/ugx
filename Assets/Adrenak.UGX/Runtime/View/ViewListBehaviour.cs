using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

namespace Adrenak.UGX {
    [Serializable]
    public abstract class ViewListBehaviour<TState, TView> : UIBehaviour where TState : ViewState where TView : View<TState> {
        public Transform container = null;
        public TView template = null;
        [SerializeField] bool useDefaultData;
        [ShowIf("useDefaultData")] public List<TState> defaultData;

        public ViewList<TState, TView> InnerList { get; private set; }

        void Start() {
            InnerList = new ViewList<TState, TView>(container, template);

            if (useDefaultData)
                InnerList.AddRange(defaultData);
        }
    }
}
