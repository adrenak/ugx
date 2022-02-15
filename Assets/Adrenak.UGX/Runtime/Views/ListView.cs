using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections;

using Object = UnityEngine.Object;

namespace Adrenak.UGX {
    /// <summary>
    /// Allows instantiation of <see cref="View{T}"/> through
    /// a List-like API.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="State"/> type that represents the state of the elements
    /// </typeparam>
    public class ListView<T> : IList<T> where T : State {
        Transform container;
        /// <summary>
        /// The <see cref="Transform"/> under which the 
        /// <see cref="View{T}"/> views will be instantiated
        /// </summary>
        public Transform Container {
            get => container;
            set => container = value;
        }

        View<T> template;
        /// <summary>
        /// The <see cref="View{T}"/> to be used as the template 
        /// for instantiation
        /// </summary>
        public View<T> Template {
            get => template;
            set {
                template = value;
                if (template.gameObject.scene != null)
                    template.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Returns the <see cref="View{T}"/> that have been instantiated
        /// so far. 
        /// </summary>
        public List<View<T>> Views { get; private set; } = new List<View<T>>();

        /// <summary>
        /// Creates an view. <see cref="Container"/> and 
        /// <see cref="Template"/> must be added before adding state data.
        /// </summary>
        public ListView() { }

        /// <summary>
        /// Creates an view with a <see cref="Container"/>. Must add
        /// <see cref="Container"/> before adding state data.
        /// </summary>
        /// <param name="container"></param>
        public ListView(Transform container) {
            Container = container;
        }

        /// <summary>
        /// Creates an view with a <see cref="Container"/> and 
        /// <see cref="Template"/>
        /// </summary>
        public ListView(Transform container, View<T> template) {
            Container = container;
            Template = template;
        }

        /// <summary>
        /// Returns the length of the state list
        /// </summary>
        public int Count => Views.Count;

        public bool IsReadOnly => false;

        /// <summary>
        /// Gets and sets the <see cref="State"/> at index
        /// </summary>
        public T this[int index] {
            get => Views[index].State;
            set => Views[index].State = value;
        }

        /// <summary>
        /// The values of the <see cref="ListView{T}"/> returned as a list.
        /// Setting this clears the views and adds the values again using 
        /// <see cref="AddRange(List{T})"/>
        /// </summary>
        public List<T> Values {
            get => Views.Select(x => x.State).ToList();
            set {
                //Clear();
                //AddRange(value);
                //return;
                for (int i = 0; i < value.Count; i++) {
                    if (i < Views.Count)
                        Views[i].State = value[i];
                    else
                        Add(value[i]);
                }
                while(Views.Count > value.Count) 
                    RemoveAt(value.Count);
            }
        }

        /// <summary>
        /// Adds a new <see cref="State"/> to the list and instantiates
        /// a <see cref="View{T}"/> for it
        /// </summary>
        public void Add(T state) {
            if (template == null)
                throw new NullReferenceException("Template can't be null");

            var view = Object.Instantiate(template, container);
            view.gameObject.SetActive(true);
            view.hideFlags = HideFlags.DontSave;
            view.State = state;

            Views.Add(view);
        }

        /// <summary>
        /// Adds an array of <see cref="State"/> to the inner list and 
        /// instantiates the <see cref="View{T}"/> objects for each.
        /// </summary>
        public List<View<T>> AddRange(T[] states) {
            var result = new List<View<T>>();
            foreach (var state in states) {
                Add(state);
                result.Add(Views[Views.Count - 1]);
            }
            return result;
        }

        /// <summary>
        /// Adds a list of <see cref="T"/> to the inner list and instantiates
        /// the <see cref="View{T}"/> objects for each.
        /// </summary>
        public List<View<T>> AddRange(List<T> items) =>
            AddRange(items.ToArray());

        /// <summary>
        /// Clears the state list and destroys all the instantiated
        /// <see cref="View{T}"/>
        /// </summary>
        public void Clear() {
            foreach (var view in Views)
                MonoBehaviour.Destroy(view.gameObject);
            Views.Clear();
        }

        /// <summary>
        /// Whether a state object has been added yet.
        /// (Will be checked against reference)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item) {
            foreach (var view in Views)
                if (view.State == item)
                    return true;
            return false;
        }

        /// <summary>
        /// Whether a state object with a given ID has been added yet.
        /// (Will be checked against <see cref="State"/> ID.
        /// </summary>
        public bool ContainsID(string id) {
            foreach (var view in Views)
                if (view.State.ID.Equals(id))
                    return true;
            return false;
        }

        /// <summary>
        /// Returns the index of a given <see cref="T"/> object in the 
        /// inner state list.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item) {
            for (int i = 0; i < Views.Count; i++) {
                var view = Views[i];
                if (view.State == item)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Removes a <see cref="State"/> object from the inner list 
        /// and destroys the <see cref="View{T}"/> object for it.
        /// </summary>
        public bool Remove(T item) {
            View<T> toBeRemoved = null;
            foreach (var view in Views)
                if (view.State == item)
                    toBeRemoved = view;
            if (toBeRemoved != null) {
                Views.Remove(toBeRemoved);
                Object.Destroy(toBeRemoved);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes a <see cref="State"/> object fromthe inner list at a given
        /// index and destroyes the <see cref="View{T}"/> for it.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public void RemoveAt(int index) {
            var toBeRemoved = Views[index];
            if (toBeRemoved != null) {
                Object.Destroy(toBeRemoved);
                Views.RemoveAt(index);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (IEnumerator<T>)new ListViewEnumerator<T>(
                Views.Select(x => x.State).ToArray()
            );
        }

        [Obsolete("Not implemented", true)]
        public void CopyTo(T[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        [Obsolete("Not implemented", true)]
        public void Insert(int index, T item) {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator() {
            return (IEnumerator<T>)new ListViewEnumerator<T>(
                Views.Select(x => x.State).ToArray()
            );
        }
    }

    // When you implement IEnumerable, you must also implement IEnumerator.
    public class ListViewEnumerator<T> : IEnumerator where T : State {
        public T[] array;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public ListViewEnumerator(T[] list) {
            array = list;
        }

        public bool MoveNext() {
            position++;
            return (position < array.Length);
        }

        public void Reset() {
            position = -1;
        }

        object IEnumerator.Current {
            get {
                return Current;
            }
        }

        public T Current {
            get {
                try {
                    return array[position];
                }
                catch (IndexOutOfRangeException) {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}