using UnityEngine;
using System.Collections.Generic;

namespace Adrenak.UPF {
    public class ViewGroupBehaviour<TModel, TView> : MonoBehaviour where TModel : Model where TView : View<TModel> {
        [SerializeField] ViewGroup<TModel, TView> group;
        public ViewGroup<TModel, TView> Group => group;

        [SerializeField] List<TModel> initialModels;

        void Start() {
            group.ModelGroup.Models.Clear();
            group.ModelGroup.Models.AddRange(initialModels);
        }
    }
}
