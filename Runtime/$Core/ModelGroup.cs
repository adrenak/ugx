using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Adrenak.UPF {
    public class ModelGroup<T> where T : Model {
        public ObservableCollection<T> Models { get; } = new ObservableCollection<T>();
        public Action<T>[] Subscriber { get; set; }
        public Action<T>[] Unsubscriber { get; set; }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.NewItems != null && Subscriber != null)
                foreach (T item in e.NewItems)
                    foreach (var subscriber in Subscriber)
                        subscriber(item);

            if (e.OldItems != null && Unsubscriber != null)
                foreach (T item in e.OldItems)
                    foreach (var unsubscriber in Unsubscriber)
                        unsubscriber(item);
        }

        public ModelGroup(Action<T>[] subscriber, Action<T>[] unsubscriber) {
            Models.CollectionChanged += OnCollectionChanged;

            Subscriber = subscriber;
            Unsubscriber = unsubscriber;
        }

        public ModelGroup(IList<T> models) {
            Models.CollectionChanged += OnCollectionChanged;

            Models.Clear();
            Models.AddRange(models);
        }

        public ModelGroup(IList<T> models, Action<T>[] subscriber, Action<T>[] unsubscriber) {
            Models.CollectionChanged += OnCollectionChanged;

            Subscriber = subscriber;
            Unsubscriber = unsubscriber;
            Models.Clear();
            Models.AddRange(models);
        }
    }
}
