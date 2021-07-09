using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections;

using Object = UnityEngine.Object;

namespace Adrenak.UGX {
    public class ViewList<T> : IEnumerable<T> where T : ViewModel {
        Transform container;
        public Transform Container {
            get => container;
            set => container = value;
        }

        View<T> template;
        public View<T> Template {
            get => template;
            set {
                template = value;
                if (template.gameObject.scene != null)
                    template.gameObject.SetActive(false);
            }
        }

        readonly List<View<T>> instances = new List<View<T>>();

        public ViewList() { }

        public ViewList(Transform container) {
            Container = container;
        }

        public ViewList(Transform container, View<T> template){
            Container = container;
            Template = template;
        }

        public View<T> this[int index] => (instances[index]);

        public int Count => instances.Count;

        public View<T> Add(T item) {
            if (template == null)
                throw new NullReferenceException("Template can't be null");

            var instance = Object.Instantiate(template, container);
            instance.gameObject.SetActive(true);
            instance.hideFlags = HideFlags.DontSave;
            instance.Model = item;

            instances.Add(instance);

            return instance;
        }

        public List<View<T>> AddRange(T[] items) {
            var result = new List<View<T>>();
            foreach (var item in items)
                result.Add(Add(item));
            return result;
        }

        public List<View<T>> AddRange(List<T> items) =>
            AddRange(items.ToArray());

        public void Clear() {
            foreach (var instance in instances)
                MonoBehaviour.Destroy(instance.gameObject);
            instances.Clear();
        }

        public bool Contains(T item) {
            foreach (var instance in instances)
                if (instance.Model == item)
                    return true;
            return false;
        }

        public IEnumerator<T> GetEnumerator() {
            return (IEnumerator<T>)new ViewListEnumerator<T>(
                instances.Select(x => x.Model).ToArray()
            );
        }

        public int IndexOf(T item) {
            for (int i = 0; i < instances.Count; i++) {
                var instance = instances[i];
                if (instance.Model == item)
                    return i;
            }
            return -1;
        }

        public bool Remove(T item) {
            View<T> toBeRemoved = null;
            foreach (var instance in instances)
                if (instance.Model == item)
                    toBeRemoved = instance;
            if (toBeRemoved != null) {
                instances.Remove(toBeRemoved);
                Object.Destroy(toBeRemoved);
                return true;
            }
            return false;
        }

        public bool RemoveAt(int index) {
            try {
                View<T> toBeRemoved = instances[index];
                if (toBeRemoved != null) {
                    Object.Destroy(toBeRemoved);
                    instances.RemoveAt(index);
                }
                return true;
            }
            catch {
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (IEnumerator<T>)new ViewListEnumerator<T>(
                instances.Select(x => x.Model).ToArray()
            );
        }

        public void CopyTo(T[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item) {
            throw new NotImplementedException();
        }
    }

    // When you implement IEnumerable, you must also implement IEnumerator.
    public class ViewListEnumerator<T> : IEnumerator where T : ViewModel {
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