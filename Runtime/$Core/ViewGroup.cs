using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Adrenak.UPF {
    public class ViewGroup<TModel, TView> where TModel : Model where TView : View<TModel> {
        public TView ViewPrefab { get; private set; }
        public Transform Container { get; private set; }

        public ModelGroup<TModel> modelGroup;

        public ObservableCollection<TModel> Models { get; } = new ObservableCollection<TModel>();
        readonly List<TView> instantiated = new List<TView>();

        public ViewGroup(Transform container, TView viewPrefab, IList<TModel> models, ModelGroup<TModel> _modelGroup) {
            ViewPrefab = viewPrefab;
            Container = container;
            modelGroup = _modelGroup;

            _modelGroup.Models.CollectionChanged += (sender, args) => {
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


            // OLD
            Models.Clear();
            Models.AddRange(models);
        }

        void Instantiate(TModel t) {
            var instance = MonoBehaviour.Instantiate(ViewPrefab, Container);
            instance.Model = t;

            instantiated.Add(instance);
        }

        void Destroy(TModel t) {
            foreach (var instance in instantiated) {
                if (instance.Model == t) {
                    MonoBehaviour.Destroy(instance.gameObject);
                }
            }
        }
    }
}
