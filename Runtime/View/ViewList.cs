using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;

namespace Adrenak.UGX {
    [Serializable]
    public class ViewList<TModel, TView> : MonoBehaviour, ICollection<TModel>, IList<TModel> where TModel : ViewModel where TView : View<TModel> {
        public Transform container = null;
        public TView template = null;
        [SerializeField] List<TModel> data = new List<TModel>();

        public List<TView> Instantiated { get; private set; } = new List<TView>();
        public int Count => data.Count;
        public bool IsReadOnly => false;
        public TModel this[int index] {
            get => data[index];
            set {
                data[index] = value;
                Instantiated[index].ViewData = value;
            }
        }

        Action<TView> onInit, onDeinit;
        public void Setup(Action<TView> onInit, Action<TView> onDeinit) {
            this.onInit = onInit;
            this.onDeinit = onDeinit;
            Instantiated.ForEach(x => this.onInit(x));
        }

        TView Instantiate(TModel t) {
            if (template == null)
                throw new Exception("No ViewTemplate assigned! Cannot instantiate elements in ViewGroup.");

            // We don't initialize when the application isn't playing.
            // This sometimes happens with requests that are fullfilled after
            // play mode exits in the editor and end up instantiation in editor mode.
            if (!Application.isPlaying)
                return null;

            var instance = Instantiate(template, container);
            instance.hideFlags = HideFlags.DontSave;
            instance.ViewData = t;

            onInit?.Invoke(instance);

            return instance;
        }

        void Destroy(TModel t) {
            foreach (var instance in Instantiated) {
                if (instance != null && instance.ViewData.Equals(t) && instance.gameObject != null) {
                    onDeinit?.Invoke(instance);
                    Instantiated.Remove(instance);
                    Destroy(instance.gameObject);
                    break;
                }
            }
        }

        public void Clear() {
            foreach (var model in data)
                Destroy(model);
            data.Clear();
        }

        public bool Contains(TModel item) {
            return data.Contains(item);
        }

        public void Add(TModel item) {
            data.Add(item);
            var instance = Instantiate(item);
            if (instance != null)
                Instantiated.Add(instance);
        }

        public void CopyTo(TModel[] array, int arrayIndex) {
            throw new NotImplementedException("ViewList doesn't support CopyTo yet.");
        }

        public bool Remove(TModel item) {
            if (Contains(item)) {
                Destroy(item);
                data.Remove(item);
                return true;
            }
            return false;
        }

        public int IndexOf(TModel item) {
            return data.IndexOf(item);
        }

        public void Insert(int index, TModel item) {
            if (item == null)
                throw new Exception("Inserted item cannot be null");

            if (index < 0 || index >= data.Count)
                throw new IndexOutOfRangeException("Insert method index was out of range");

            data.Insert(index, item);
            var instance = Instantiate(item);
            Instantiated.Insert(index, instance);
        }

        public void RemoveAt(int index) {
            if (index < 0 || index >= data.Count)
                throw new IndexOutOfRangeException("RemoveAt method index was out of range");

            var vm = data[index];
            Destroy(vm);
            data.RemoveAt(index);
        }

        public IEnumerator<TModel> GetEnumerator() {
            return (IEnumerator<TModel>)new ViewGroupEnum<TModel>(data.ToArray());
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (IEnumerator<TModel>)new ViewGroupEnum<TModel>(data.ToArray());
        }
    }

    // When you implement IEnumerable, you must also implement IEnumerator.
    public class ViewGroupEnum<T> : IEnumerator where T : ViewModel {
        public T[] array;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public ViewGroupEnum(T[] list) {
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
