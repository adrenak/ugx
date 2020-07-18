using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Adrenak.UPF {
    public class ViewModelGroup<T> where T : ViewModel {
        public ObservableCollection<T> ViewModels { get; } = new ObservableCollection<T>();

        public Action<T>[] Subscribers { get; set; }
        public Action<T> PrimarySubscriber {
            get {
                if(Subscribers != null && Subscribers.Length > 0)
                    return Subscribers[0];
                return null;
            }
            set {
                if (Subscribers == null)
                    Subscribers = new Action<T>[] { value };
                else
                    Subscribers[0] = value;
            }
        }
        public Action<T>[] Unsubscribers { get; set; }
        public Action<T> PrimaryUnsubscriber {
            get {
                if (Unsubscribers != null && Unsubscribers.Length > 0)
                    return Unsubscribers[0];
                return null;
            }
            set {
                if (Unsubscribers == null)
                    Unsubscribers = new Action<T>[] { value };
                else
                    Unsubscribers[0] = value;
            }
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.NewItems != null && Subscribers != null)
                foreach (T item in e.NewItems)
                    foreach (var subscriber in Subscribers)
                        subscriber(item);

            if (e.OldItems != null && Unsubscribers != null)
                foreach (T item in e.OldItems)
                    foreach (var unsubscriber in Unsubscribers)
                        unsubscriber(item);
        }

        public ViewModelGroup() {
            ViewModels.CollectionChanged += OnCollectionChanged;
        }

        public ViewModelGroup(IList<T> models) {
            ViewModels.CollectionChanged += OnCollectionChanged;

            ViewModels.Clear();
            ViewModels.AddRange(models);
        }
    }
}
