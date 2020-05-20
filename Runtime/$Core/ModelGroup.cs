using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Adrenak.UPF {
    public class ModelGroup<T> where T : Model {
        public ObservableCollection<T> Models { get; } = new ObservableCollection<T>();
        Action<T>[] m_Subscriber;
        Action<T>[] n_Unsubscriber;

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.NewItems != null && m_Subscriber != null)
                foreach (T item in e.NewItems)
                    foreach (var subscriber in m_Subscriber)
                        subscriber(item);

            if (e.OldItems != null && n_Unsubscriber != null)
                foreach (T item in e.OldItems)
                    foreach (var unsubscriber in n_Unsubscriber)
                        unsubscriber(item);
        }

        public ModelGroup(Action<T>[] subscriber, Action<T>[] unsubscriber) {
            Models.CollectionChanged += OnCollectionChanged;

            m_Subscriber = subscriber;
            n_Unsubscriber = unsubscriber;
        }

        public ModelGroup(IList<T> models) {
            Models.CollectionChanged += OnCollectionChanged;

            Models.Clear();
            Models.AddRange(models);
        }

        public ModelGroup(IList<T> models, Action<T>[] subscriber, Action<T>[] unsubscriber) {
            Models.CollectionChanged += OnCollectionChanged;

            m_Subscriber = subscriber;
            n_Unsubscriber = unsubscriber;
            Models.Clear();
            Models.AddRange(models);
        }
    }
}
