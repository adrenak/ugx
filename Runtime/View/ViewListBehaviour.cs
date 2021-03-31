using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

namespace Adrenak.UGX {
    [Serializable]
    public abstract class ViewListBehaviour<TState, TView> : UGXBehaviour where TState : ViewState where TView : View<TState> {
        public Transform container = null;
        public TView template = null;

        [SerializeField] bool useDefaultStates = false;
        [ShowIf("useDefaultStates")] public List<TState> defaultStates;

        public ViewList<TState, TView> InnerList { get; private set; }

        new void Awake() {
            base.Awake();
            InnerList = new ViewList<TState, TView>(container, template);

            if (useDefaultStates)
                InnerList.AddRange(defaultStates);
        }

        [Button]
        public void Refresh(){
            if (useDefaultStates){
                InnerList.Clear();
                InnerList.AddRange(defaultStates);
            }
        }
    }
}
