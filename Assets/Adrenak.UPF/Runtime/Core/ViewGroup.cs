using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;
using Object = UnityEngine.Object;
using System.Collections.ObjectModel;

namespace Adrenak.UPF {
    [Serializable]
    public class ViewGroup<TModel, TView> where TModel : ViewModel where TView : View<TModel> {
        public Transform Container { get; private set; }
        public TView ViewTemplate { get; private set; }
        
        [Obsolete("Access the models directly via .ViewModels. Access to the view model group may be removed soon.")]
        public ViewModelGroup<TModel> ViewModelGroup { get; private set; }        

        public ObservableCollection<TModel> ViewModels => ViewModelGroup.ViewModels;

        readonly List<TView> instantiated = new List<TView>();

        [Obsolete]
        public ViewGroup(Transform container, TView viewTemplate, ViewModelGroup<TModel> modelGroup) {
            ViewTemplate = viewTemplate;
            Container = container;
            ViewModelGroup = modelGroup;
            ViewModelGroup.ViewModels.CollectionChanged += (sender, args) => {
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

            foreach (var model in ViewModelGroup.ViewModels)
                Instantiate(model as TModel);
        }

        [Obsolete]
        public ViewGroup(Transform container, TView viewTemplate, IList<TModel> models) {
            // For some reason I cannot call this here:
            //return new ViewGroup<TModel, TView>(container, viewTemplate, new ModelGroup<TModel>(models));
            // When I do that, the ModelGroup becomes null after the actual constructor is done. 
            // Looks like I'm missing something.
            ViewTemplate = viewTemplate;
            Container = container;
            ViewModelGroup = new ViewModelGroup<TModel>(models);
            ViewModelGroup.ViewModels.CollectionChanged += (sender, args) => {
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

            foreach (var model in ViewModelGroup.ViewModels)
                Instantiate(model as TModel);
        }

        public ViewGroup(Transform container, TView viewTemplate) {
            ViewTemplate = viewTemplate;
            Container = container;
            ViewModelGroup = new ViewModelGroup<TModel>();
            ViewModelGroup.ViewModels.CollectionChanged += (sender, args) => {
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
