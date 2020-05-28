using UnityEngine;
using System.Collections.Generic;

namespace Adrenak.UPF {
    public class ViewGroupObject<TModel, TView> : MonoBehaviour where TModel : ViewModel where TView : View<TModel> {
        public ViewGroup<TModel, TView> InnerViewGroup { get; private set; }

        [SerializeField] Transform container;
        [SerializeField] TView template;
        [SerializeField] List<TModel> initialModels;

        void Start() {
            InnerViewGroup = new ViewGroup<TModel, TView>(container, template);
            InnerViewGroup.ModelGroup.Models.Clear();
            InnerViewGroup.ModelGroup.Models.AddRange(initialModels);
        }
    }
}
