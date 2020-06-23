using UnityEngine;
using System.Collections.Generic;

namespace Adrenak.UPF {
    public class ViewGroupObject<TModel, TView> : MonoBehaviour where TModel : ViewModel where TView : View<TModel> {
        public ViewGroup<TModel, TView> InnerViewGroup { get; private set; }

#pragma warning disable 0649
        [SerializeField] Transform container;
        [SerializeField] TView template;
        [SerializeField] List<TModel> initialModels;
#pragma warning restore 0649

        void Start() {
            InnerViewGroup = new ViewGroup<TModel, TView>(container, template);
            InnerViewGroup.ViewModelGroup.ViewModels.Clear();
            InnerViewGroup.ViewModelGroup.ViewModels.AddRange(initialModels);
        }
    }
}
