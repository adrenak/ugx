using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

namespace Adrenak.UGX {
    [Serializable]
    public abstract class ViewListBehaviour<TModel, TView> : UIBehaviour where TModel : ViewModel where TView : View<TModel> {
        public Transform container = null;
        public TView template = null;
        [SerializeField] bool useDefaultData;
        [ShowIf("useDefaultData")] public List<TModel> defaultData;

        public ViewList<TModel, TView> InnerList { get; private set; }

        void Start() {
            InnerList = new ViewList<TModel, TView>(container, template);

            if (useDefaultData)
                InnerList.AddRange(defaultData);
        }
    }
}
