using System;
using Adrenak.UPF;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using UnityEngine;


public class ModelGroup<T> where T : Model {
    public ObservableCollection<T> Models { get; } = new ObservableCollection<T>();
    public List<Action<T>> subscribers;
    public List<Action<T>> unsubscribers;

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
        if (e.NewItems != null)
            foreach (T item in e.NewItems)
                foreach (var subscriber in subscribers)
                    subscriber(item);

        if (e.OldItems != null)
            foreach (T item in e.OldItems)
                foreach (var unsubscriber in unsubscribers)
                    unsubscriber(item);
    }

    public ModelGroup(List<Action<T>> subscribers, List<Action<T>> unsubscribers) {
        Models.CollectionChanged += OnCollectionChanged;
        this.subscribers = subscribers;
        this.unsubscribers = unsubscribers;
    }

    //public void AddListener(Func<T, EventArgs>, EventHandler subscriber) {
    //    foreach (var item in list) {
    //        var extracted = typeof(T).GetEvent(eventName);

    //        if (!listenerMap.ContainsKey(nameof(extracted))) {
    //            listenerMap.Add(nameof(extracted), new List<EventHandler> { subscriber });
    //            extracted += (sender, e) => {
    //                Debug.Log("s");
    //                foreach (var listener in listenerMap[nameof(extracted)])
    //                    listener?.Invoke(item, e);
    //            };
    //        }
    //        else
    //            listenerMap[nameof(extracted)].Add(subscriber);
    //    }
    //}
}
//public abstract class ManagedEventCollection<T, TEventArgs> : IList<T> {
//    private EventInfo m_event;
//    ObservableCollection<T> m_list;

//    public ManagedEventCollection(string eventName) {
//        m_list = new ObservableCollection<T>();
//        m_event = typeof(T).GetEvent(eventName);

//        m_list.CollectionChanged += CollectionChanged;
//    }

//    private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs ea) {
//        foreach (T item in ea.NewItems) {
//            Debug.Log(Handler.Method);
//            m_event.AddEventHandler(item, Delegate.CreateDelegate(m_event.EventHandlerType, Handler.Method));
//        }
//        foreach (T item in ea.OldItems) {
//            m_event.RemoveEventHandler(
//               item,
//               Delegate.CreateDelegate(m_event.EventHandlerType, item, Handler.Method));
//        }
//    }

//    public T this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

//    // Add/Remove/Indexer/Clear methods alter contents of m_list.

//    public EventHandler<TEventArgs> Handler { get; set; }

//    public int Count => throw new NotImplementedException();

//    public bool IsReadOnly => throw new NotImplementedException();

//    public void Add(T item) {
//        m_list.Add(item);
//    }

//    public void Clear() {
//        throw new NotImplementedException();
//    }

//    public bool Contains(T item) {
//        throw new NotImplementedException();
//    }

//    public void CopyTo(T[] array, int arrayIndex) {
//        throw new NotImplementedException();
//    }

//    public IEnumerator<T> GetEnumerator() {
//        throw new NotImplementedException();
//    }

//    public int IndexOf(T item) {
//        throw new NotImplementedException();
//    }

//    public void Insert(int index, T item) {
//        throw new NotImplementedException();
//    }

//    public bool Remove(T item) {
//        throw new NotImplementedException();
//    }

//    public void RemoveAt(int index) {
//        throw new NotImplementedException();
//    }

//    IEnumerator IEnumerable.GetEnumerator() {
//        throw new NotImplementedException();
//    }
//}