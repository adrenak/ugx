using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;
using Object = UnityEngine.Object;
using System.Collections.ObjectModel;

namespace Adrenak.UPF {
    [Serializable]
    public class ViewGroup<TModel> where TModel : ViewModel {
        public Transform Container { get; private set; }
        public View<TModel> ViewTemplate { get; set; }
        public ObservableCollection<TModel> ViewModels { get; private set; } = new ObservableCollection<TModel>();

        readonly List<View<TModel>> instantiated = new List<View<TModel>>();

        public ViewGroup(Transform container, View<TModel> viewTemplate = null) {
            ViewTemplate = viewTemplate;
            Container = container;

            ViewModels.CollectionChanged += (sender, args) => {
                switch (args.Action) {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var newItem in args.NewItems){
                            Instantiate(newItem as TModel);
                            ViewModelInit?.Invoke(newItem as TModel);
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var removedItem in args.OldItems){
                            Destroy(removedItem as TModel);
                            ViewModelDeinit?.Invoke(removedItem as TModel);
                        }
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        foreach (var instance in instantiated){
                            Destroy(instance.ViewModel);
                            ViewModelDeinit?.Invoke(instance as TModel);
                        }
                        break;
                }
            };
        }

        public Action<TModel> ViewModelInit, ViewModelDeinit;

        void Instantiate(TModel t) {
            if (ViewTemplate == null)
                throw new Exception("No ViewTemplate assigned! Cannot instantiate elements in ViewGroup.");

            // We don't initialize when the application isn't playing.
            // This sometimes happens with requests that are fullfilled after
            // play mode exits in the editor and end up instantiation in editor mode.
            if (!Application.isPlaying) 
                return;

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
