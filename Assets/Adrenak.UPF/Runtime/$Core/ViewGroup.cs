using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Adrenak.UPF {
    public class ViewGroup<TModel, TView> where TModel : Model where TView : View<TModel> {
        public TView ViewPrefab { get; private set; }
        public Transform Container { get; private set; }

        public ModelGroup<TModel> ModelGroup { get; private set; }

        readonly List<TView> instantiated = new List<TView>();

        public ViewGroup(Transform container, TView viewPrefab, ModelGroup<TModel> _modelGroup) {
            ViewPrefab = viewPrefab;
            Container = container;
            ModelGroup = _modelGroup;

            ModelGroup.Models.CollectionChanged += (sender, args) => {
                switch (args.Action) {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var newItem in args.NewItems)
                            Instantiate(newItem as TModel);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var removed in args.OldItems)
                            Destroy(removed as TModel);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        foreach (var instance in instantiated)
                            Destroy(instance.Model);
                        break;
                }
            };

            foreach (var model in _modelGroup.Models)
                Instantiate(model as TModel);
        }

        void Instantiate(TModel t) {
            var instance = Object.Instantiate(ViewPrefab, Container);
            instance.Model = t;

            instantiated.Add(instance);
        }

        void Destroy(TModel t) {
            foreach (var instance in instantiated) {
                if (instance.Model == t)
                    Object.Destroy(instance.gameObject);
            }
        }
    }
}
