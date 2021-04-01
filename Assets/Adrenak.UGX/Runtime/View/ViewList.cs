using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Adrenak.UGX {
    [Serializable]
    public class ViewList<TState, TView> : ICollection<TState>, IList<TState> where TState : ViewState where TView : View<TState> {
        public Transform container = null;
        public TView template = null;
        [SerializeField] List<TState> states = new List<TState>();

        public List<TView> Instantiated { get; private set; } = new List<TView>();
        public int Count => states.Count;
        public bool IsReadOnly => false;
        public TState this[int index] {
            get => states[index];
            set {
                states[index] = value;
                Instantiated[index].CurrentState = value;
            }
        }

        public ViewList(Transform container, TView template) {
            this.container = container;
            this.template = template;
        }

        Action<TView> onInit, onDeinit;
        public void Setup(Action<TView> onInit, Action<TView> onDeinit) {
            this.onInit = onInit;
            this.onDeinit = onDeinit;
            Instantiated.ForEach(x => this.onInit(x));
        }

        TView Instantiate(TState t) {
            if (template == null)
                throw new Exception("No ViewTemplate assigned! Cannot instantiate elements in ViewGroup.");

            // We don't initialize when the application isn't playing.
            // This sometimes happens with requests that are fullfilled after
            // play mode exits in the editor and end up instantiation in editor mode.
            if (!Application.isPlaying)
                return null;

            var instance = MonoBehaviour.Instantiate(template, container);
            if (!instance.gameObject.activeSelf)
                instance.gameObject.SetActive(true);
            instance.hideFlags = HideFlags.DontSave;
            instance.CurrentState = t;

            onInit?.Invoke(instance);

            return instance;
        }

        void Destroy(TState t) {
            foreach (var instance in Instantiated) {
                if (instance != null && instance.CurrentState.Equals(t) && instance.gameObject != null) {
                    onDeinit?.Invoke(instance);
                    Instantiated.Remove(instance);
                    MonoBehaviour.Destroy(instance.gameObject);
                    break;
                }
            }
        }

        public void Clear() {
            foreach (var state in states)
                Destroy(state);
            states.Clear();
        }

        public bool Contains(TState item) {
            return states.Contains(item);
        }

        public void Add(TState item) {
            states.Add(item);
            var instance = Instantiate(item);
            if (instance != null)
                Instantiated.Add(instance);
        }

        public void CopyTo(TState[] array, int arrayIndex) {
            throw new NotImplementedException("ViewList doesn't support CopyTo yet.");
        }

        public bool Remove(TState item) {
            if (Contains(item)) {
                Destroy(item);
                states.Remove(item);
                return true;
            }
            return false;
        }

        public int IndexOf(TState item) {
            return states.IndexOf(item);
        }

        public void Insert(int index, TState item) {
            if (item == null)
                throw new Exception("Inserted item cannot be null");

            if (index < 0 || index >= states.Count)
                throw new IndexOutOfRangeException("Insert method index was out of range");

            states.Insert(index, item);
            var instance = Instantiate(item);
            Instantiated.Insert(index, instance);
        }

        public void RemoveAt(int index) {
            if (index < 0 || index >= states.Count)
                throw new IndexOutOfRangeException("RemoveAt method index was out of range");

            var vm = states[index];
            Destroy(vm);
            states.RemoveAt(index);
        }

        public IEnumerator<TState> GetEnumerator() {
            return (IEnumerator<TState>)new ViewListEnumerator<TState>(states.ToArray());
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (IEnumerator<TState>)new ViewListEnumerator<TState>(states.ToArray());
        }
    }

    // When you implement IEnumerable, you must also implement IEnumerator.
    public class ViewListEnumerator<T> : IEnumerator where T : ViewState {
        public T[] array;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public ViewListEnumerator(T[] list) {
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
