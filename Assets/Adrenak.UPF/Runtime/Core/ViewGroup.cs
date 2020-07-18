using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;
using Object = UnityEngine.Object;

namespace Adrenak.UPF {
    [Serializable]
    public class ViewGroup<TModel, TView> where TModel : ViewModel where TView : View<TModel> {
        public Transform Container { get; private set; }
        public TView ViewTemplate { get; set; }
        public ViewModelGroup<TModel> ViewModelGroup { get; private set; }

        readonly List<TView> instantiated = new List<TView>();

        public ViewGroup(Transform container, TView viewTemplate = null) {
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
                        foreach (var removedItem in args.OldItems)
                            Destroy(removedItem as TModel);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        foreach (var instance in instantiated)
                            Destroy(instance.ViewModel);
                        break;
                }
            };
        }

        void Instantiate(TModel t) {
            if (ViewTemplate == null)
                throw new Exception("No ViewTemplate assigned! Cannot instantiate elements in ViewGroup.");

            var instance = Object.Instantiate(ViewTemplate, Container);
            instance.hideFlags = HideFlags.DontSave;
            instance.ViewModel = t;
            instantiated.Add(instance);
        }

        void Destroy(TModel t) {
            foreach (var instance in instantiated)
                if (instance != null && instance.ViewModel.Equals(t) && instance.gameObject != null)
                    Object.Destroy(instance.gameObject);
        }
    }
}
