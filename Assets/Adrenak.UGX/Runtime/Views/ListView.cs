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
        List<View<T>> available = new List<View<T>>();

        View<T> Get() {
            if (available.Count == 0) {
                Create();
                return Get();
            }
            var toReturn = available[0];
            available.RemoveAt(0);
            return toReturn;
        }

        void Free(View<T> instance) {
            if(!available.Contains(instance))
                available.Add(instance);
        }

        void Create() {
            var newInstance = MonoBehaviour.Instantiate(Template, container);
            available.Add(newInstance);
        }

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
                template.AutoInitializeOnStart = false;
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
        /// Adds a new <see cref="State"/> to the list by ensuring a 
        /// <see cref="View{TState}"/> for it
        /// </summary>
        public void Add(T state) {
            if (template == null)
                throw new NullReferenceException("Template can't be null");

            //var view = Object.Instantiate(template, container);
            var view = Get();
            view.State = state;
            view.gameObject.SetActive(true);
            view.hideFlags = HideFlags.DontSave;

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
            foreach (var view in Views) {
                Free(view);
                view.gameObject.SetActive(false);
            }
            Views.Clear();
        }

        /// <summary>
        /// Returns the <see cref="View{T}"/> object of the 
        /// <see cref="T"/> element with the given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>False if no state with this ID</returns>
        public View<T> GetByID(string id) {
            foreach (var view in Views)
                if (view.State.ID == id)
                    return view;
            return null;
        }

        /// <summary>
        /// Returns whether the inner list contains a <see cref="T"/> 
        /// (uses <see cref="T.ID"/> for comparision
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item) => ContainsID(item.ID);

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
        public bool Remove(T item) => RemoveByID(item.ID);

        /// <summary>
        /// Removes a <see cref="State"/> object from the inner ist
        /// using the ID and destroys the <see cref="View{T}"/> 
        /// object for it.
        /// </summary>
        /// <param name="id">The ID of the state that should be removed</param>
        public bool RemoveByID(string id) {
            if (!ContainsID(id)) return false;
            View<T> toBeRemoved = null;
            foreach (var view in Views) {
                if (view.State.ID.Equals(id)) {
                    toBeRemoved = view;
                    break;
                }
            }
            if(toBeRemoved != null) {
                Views.Remove(toBeRemoved);
                toBeRemoved.gameObject.SetActive(false);
                Free(toBeRemoved);
                //Object.Destroy(toBeRemoved.gameObject);
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
                toBeRemoved.gameObject.SetActive(false);
                Free(toBeRemoved);
                //Object.Destroy(toBeRemoved.gameObject);
                Views.RemoveAt(index);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        [Obsolete("Not implemented", true)]
        public void CopyTo(T[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        [Obsolete("Not implemented", true)]
        public void Insert(int index, T item) {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator() =>
            Views.Select(x => x.State).GetEnumerator();
    }
}