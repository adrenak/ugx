using UnityEngine;
using System.Collections.Generic;

namespace Adrenak.UPF {
    public abstract class ViewGroupObject<TModel, TView> : MonoBehaviour where TModel : ViewModel where TView : View<TModel> {
        public ViewGroup<TModel> ViewGroup { get; private set; }

#pragma warning disable 0649
        [SerializeField] Transform container;
        [SerializeField] TView template;
        [SerializeField] List<TModel> models;
#pragma warning restore 0649

        void Start() {
            ViewGroup = new ViewGroup<TModel>(container, template);
            ViewGroup.ViewModels.Clear();
            ViewGroup.ViewModels.AddRange(models);
        }
    }
}
