using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;
using Object = UnityEngine.Object;

namespace Adrenak.UPF {
    [Serializable]
    public class ViewGroup<TModel, TView> where TModel : Model where TView : View<TModel> {
        public Transform Container { get; private set; }
        public TView ViewTemplate { get; private set; }
        public ModelGroup<TModel> ModelGroup { get; private set; }

        readonly List<TView> instantiated = new List<TView>();

        public ViewGroup(Transform container, TView viewTemplate, ModelGroup<TModel> modelGroup) {
            ViewTemplate = viewTemplate;
            Container = container;
            ModelGroup = modelGroup;
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

            foreach (var model in ModelGroup.Models)
                Instantiate(model as TModel);
        }

        public ViewGroup(Transform container, TView viewTemplate, IList<TModel> models) {
            // For some reason I cannot call this here:
            //return new ViewGroup<TModel, TView>(container, viewTemplate, new ModelGroup<TModel>(models));
            // When I do that, the ModelGroup becomes null after the actual constructor is done. 
            // Looks like I'm missing something.
            ViewTemplate = viewTemplate;
            Container = container;
            ModelGroup = new ModelGroup<TModel>(models);
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

            foreach (var model in ModelGroup.Models)
                Instantiate(model as TModel);
        }

        public ViewGroup(Transform container, TView viewTemplate) {
            ViewTemplate = viewTemplate;
            Container = container;
            ModelGroup = new ModelGroup<TModel>();
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
        }

        void Instantiate(TModel t) {
            var instance = Object.Instantiate(ViewTemplate, Container);
            instantiated.Add(instance);
            instance.Model = t;
        }

        void Destroy(TModel t) {
            foreach (var instance in instantiated) 
                if (instance.Model == t)
                    Object.Destroy(instance.gameObject);
        }
    }
}
