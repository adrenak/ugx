using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Adrenak.UPF {
    public class ModelGroup<T> where T : Model {
        public ObservableCollection<T> Models { get; } = new ObservableCollection<T>();
        Action<T>[] sub;
        Action<T>[] unsub;

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.NewItems != null && sub != null)
                foreach (T item in e.NewItems)
                    foreach (var s in sub)
                        s(item);

            if (e.OldItems != null && unsub != null)
                foreach (T item in e.OldItems)
                    foreach (var u in unsub)
                        u(item);
        }

        public ModelGroup(Action<T>[] subscriber, Action<T>[] unsubscriber) {
            Models.CollectionChanged += OnCollectionChanged;

            sub = subscriber;
            unsub = unsubscriber;
        }

        public ModelGroup(IList<T> models) {
            Models.CollectionChanged += OnCollectionChanged;

            Models.Clear();
            Models.AddRange(models);
        }

        public ModelGroup(IList<T> models, Action<T>[] subscriber, Action<T>[] unsubscriber) {
            Models.CollectionChanged += OnCollectionChanged;

            sub = subscriber;
            unsub = unsubscriber;
            Models.Clear();
            Models.AddRange(models);
        }
    }
}
